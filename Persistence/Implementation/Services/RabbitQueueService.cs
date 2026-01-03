using Application.Contracts.Services;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Persistence.Implementation.Services
{
    internal class RabbitQueueService(IChannel _queueChannel, IConfiguration _config, ILogger<RabbitQueueService> _logger) : IQueueService
    {
        public async Task PublishChange(Apartment apartment, ApartmentChangeEnum apartmentChange)
        {
            try
            {
                await DeclareMessageingChannels();

                var json = JsonSerializer.Serialize(new
                {
                    apartment,
                    apartmentChange = apartmentChange.ToString()
                });

                var body = Encoding.UTF8.GetBytes(json);

                await _queueChannel.BasicPublishAsync(_config.GetValue<string>("RabbitMQ:ExchangeName"), "", body);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                _logger.LogError(ex, ex.Message);
            }
        }

        private async Task DeclareMessageingChannels()
        {
            await _queueChannel.ExchangeDeclareAsync(_config.GetValue<string>("RabbitMQ:ExchangeName"), ExchangeType.Fanout);
            await _queueChannel.QueueDeclareAsync(_config.GetValue<string>("RabbitMQ:SearchQueueName"));
            await _queueChannel.QueueDeclareAsync(_config.GetValue<string>("RabbitMQ:BookingQueueName"));
            await _queueChannel.QueueBindAsync(_config.GetValue<string>("RabbitMQ:SearchQueueName"), _config.GetValue<string>("RabbitMQ:ExchangeName"), "");
            await _queueChannel.QueueBindAsync(_config.GetValue<string>("RabbitMQ:BookingQueueName"), _config.GetValue<string>("RabbitMQ:ExchangeName"), "");
        }
    }
}
