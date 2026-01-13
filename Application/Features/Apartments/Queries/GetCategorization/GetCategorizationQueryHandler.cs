using Application.Contracts.Repos;
using Application.Response;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace Application.Features.Apartments.Queries.GetCategorization
{
    internal class GetCategorizationQueryHandler(IBaseRepo<Apartment> _apartmentRepo, IConfiguration _configuration)
        : IRequestHandler<GetCategorizationQuery, ApiResponse<GetCategorizationQueryResponse>>
    {
        public async Task<ApiResponse<GetCategorizationQueryResponse>> Handle(GetCategorizationQuery request, CancellationToken cancellationToken)
        {
            var apartments = await _apartmentRepo.GetAllAsync();
            if (apartments == null || !apartments.Any())
                return ApiResponse<GetCategorizationQueryResponse>.GetNotFoundApiResponse("No apartments found.");
            try
            {
                (HttpClient client, List<CategorizationDto> categorization) = await CallUnibzModel(apartments);

                return ApiResponse<GetCategorizationQueryResponse>.GetSuccessApiResponse(new()
                {
                    CategorizedApartments = categorization
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            (HttpClient localClient, List<CategorizationDto> localCategorization) = await CallOllamaModel(apartments);

            return ApiResponse<GetCategorizationQueryResponse>.GetSuccessApiResponse(new()
            {
                CategorizedApartments = localCategorization
            });

        }

        private async Task<(HttpClient client, List<CategorizationDto> categorization)> CallOllamaModel(IEnumerable<Apartment> apartments)
        {
            var url = "http://host.docker.internal:11434/api/chat";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();

            // Build prompt dynamically from apartments
            var sb = new StringBuilder();
            sb.AppendLine("Classify each apartment into a new category. Respond strictly with valid JSON array of objects:");
            sb.AppendLine("[{ \"ApartmentId\": \"...\", \"Category\": \"...\" }]");
            foreach (var apt in apartments)
            {
                sb.AppendLine($"ApartmentId: {apt.Id}, Name: {apt.Name}, Address: {apt.Address}, Floor: {apt.Floor}, NoiseLevel: {apt.NoiseLevel}, Area: {apt.AreaInSquareMeters} m², PricePerDay: {apt.PricePerDay}");
            }

            var modelRequest = new
            {
                model = "gpt-oss:120b-cloud", // or whichever local model you want
                messages = new List<object>
        {
            new { role = "user", content = sb.ToString() }
        }
            };

            var json = JsonSerializer.Serialize(modelRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Important: use ResponseHeadersRead so we can stream line by line
            using var response = await client.PostAsync(url, content);
            Console.WriteLine("Status: " + (int)response.StatusCode);

            var categorization = new List<CategorizationDto>();
            var sbOutput = new StringBuilder();

            using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line)) continue;

                try
                {
                    using var doc = JsonDocument.Parse(line);
                    if (doc.RootElement.TryGetProperty("message", out var msg) &&
                        msg.TryGetProperty("content", out var contentProp))
                    {
                        sbOutput.Append(contentProp.GetString());
                    }

                    if (doc.RootElement.TryGetProperty("done", out var doneProp) &&
                        doneProp.GetBoolean())
                    {
                        break; // finished
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Streaming parse error: " + ex.Message);
                }
            }

            var assistantContent = sbOutput.ToString();
            Console.WriteLine("Aggregated assistant output:");
            Console.WriteLine(assistantContent);

            if (!string.IsNullOrEmpty(assistantContent))
            {
                try
                {
                    categorization = JsonSerializer.Deserialize<List<CategorizationDto>>(assistantContent,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<CategorizationDto>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Deserialization error: " + ex.Message);
                }
            }

            Console.WriteLine("Categorization:");
            foreach (var cat in categorization)
            {
                Console.WriteLine($"ApartmentId: {cat.ApartmentId}, Category: {cat.Category}");
            }

            return (client, categorization);
        }


        private async Task<(HttpClient client, List<CategorizationDto> categorization)> CallUnibzModel(IEnumerable<Apartment> apartments)
        {
            var url = _configuration.GetValue<string>("Ollama:Url");
            var token = _configuration.GetValue<string>("Ollama:Token");

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            // Build prompt dynamically from apartments
            var sb = new StringBuilder();
            sb.AppendLine("Classify each apartment into a new category. Respond strictly with valid JSON array of objects:");
            sb.AppendLine("[{ \"ApartmentId\": \"...\", \"Category\": \"...\" }]");
            foreach (var apt in apartments)
            {
                sb.AppendLine($"ApartmentId: {apt.Id}, Name: {apt.Name}, Address: {apt.Address}, Floor: {apt.Floor}, NoiseLevel: {apt.NoiseLevel}, Area: {apt.AreaInSquareMeters} m², PricePerDay: {apt.PricePerDay}");
            }

            var modelRequest = new
            {
                model = "llama3:latest",
                messages = new List<object>
        {
            new { role = "user", content = sb.ToString() }
        }
            };

            var json = JsonSerializer.Serialize(modelRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);
            Console.WriteLine("Status: " + (int)response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Raw response:");
            Console.WriteLine(responseBody);

            var categorization = new List<CategorizationDto>();
            try
            {
                using var doc = JsonDocument.Parse(responseBody);
                var choices = doc.RootElement.GetProperty("choices");
                if (choices.GetArrayLength() > 0)
                {
                    var message = choices[0].GetProperty("message");
                    var assistantContent = message.GetProperty("content").GetString();

                    Console.WriteLine("Assistant content:");
                    Console.WriteLine(assistantContent);

                    // Strip any explanatory text before the JSON array
                    var startIndex = assistantContent.IndexOf('[');
                    var endIndex = assistantContent.LastIndexOf(']');
                    if (startIndex >= 0 && endIndex > startIndex)
                    {
                        var jsonArray = assistantContent.Substring(startIndex, endIndex - startIndex + 1);
                        categorization = JsonSerializer.Deserialize<List<CategorizationDto>>(jsonArray,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<CategorizationDto>();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Parsing error: " + ex.Message);
            }

            Console.WriteLine("Categorization:");
            foreach (var cat in categorization)
            {
                Console.WriteLine($"ApartmentId: {cat.ApartmentId}, Category: {cat.Category}");
            }

            return (client, categorization);
        }
    }
}
