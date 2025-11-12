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
                var json = JsonSerializer.Serialize(new
                {
                    apartment,
                    apartmentChange = apartmentChange.ToString()
                });

                var body = Encoding.UTF8.GetBytes(json);

                await _queueChannel.BasicPublishAsync("", _config.GetValue<string>("RabbitMQ:QueueName"), body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
