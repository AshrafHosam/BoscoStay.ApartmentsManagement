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
            // Ollama local endpoint
            var url = "http://host.docker.internal:11434/api/chat";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();

            // Build prompt dynamically from apartments
            var sb = new StringBuilder();
            sb.AppendLine("Classify each apartment into a new category. Return JSON list with each object:");
            sb.AppendLine("ApartmentId (GUID) and Category (string).");
            foreach (var apt in apartments)
            {
                sb.AppendLine($"ApartmentId: {apt.Id}, Name: {apt.Name}, Address: {apt.Address}, Floor: {apt.Floor}, NoiseLevel: {apt.NoiseLevel}, Area: {apt.AreaInSquareMeters} m², PricePerDay: {apt.PricePerDay}");
            }

            // Ollama chat API expects: { "model": "...", "messages": [...] }
            var modelRequest = new
            {
                model = "gpt-oss:120b-cloud",
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
            Console.WriteLine("Response:");
            Console.WriteLine(responseBody);

            // Ollama returns a streaming JSON by default. If you want the final message only,
            // you may need to parse the "message" field from the last object.
            // Example: { "model":"...", "message":{"role":"assistant","content":"..."} }

            // Extract the assistant's content
            string? assistantContent = null;
            try
            {
                using var doc = JsonDocument.Parse(responseBody);
                if (doc.RootElement.TryGetProperty("message", out var msg) &&
                    msg.TryGetProperty("content", out var contentProp))
                {
                    assistantContent = contentProp.GetString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Parsing error: " + ex.Message);
            }

            var categorization = new List<CategorizationDto>();
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
            sb.AppendLine("Classify each apartment into a new category. Return JSON list with each object:");
            sb.AppendLine("ApartmentId (GUID) and Category (string).");
            foreach (var apt in apartments)
            {
                sb.AppendLine($"ApartmentId: {apt.Id}, Name: {apt.Name}, Address: {apt.Address}, Floor: {apt.Floor}, NoiseLevel: {apt.NoiseLevel}, Area: {apt.AreaInSquareMeters} m², PricePerDay: {apt.PricePerDay}");
            }

            var modelRequest = new
            {
                model = "llama3:latest",
                messages = new List<object>
                {
                    new  { role = "user", content = sb.ToString() }
                }
            };

            var json = JsonSerializer.Serialize(modelRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);
            Console.WriteLine("Status: " + (int)response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Response:");
            Console.WriteLine(responseBody);

            var categorization = JsonSerializer.Deserialize<List<CategorizationDto>>(responseBody,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            Console.WriteLine("Categorization:");
            foreach (var cat in categorization)
            {
                Console.WriteLine($"ApartmentId: {cat.ApartmentId}, Category: {cat.Category}");
            }

            return (client, categorization);
        }
    }
}
