using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace Persistence.Implementation.BackgroundServices
{
    public class RabbitMQConfigurationsService : BackgroundService
    {
        private readonly IChannel _queueChannel;
        private readonly IConfiguration _config;
        private bool _isInitialized = false;
        public RabbitMQConfigurationsService(IChannel queueChannel, IConfiguration config)
        {
            _queueChannel = queueChannel;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested == false && _isInitialized == false)
            {
                try
                {
                    Console.WriteLine("Initializing RabbitMQ");
                    await DeclareMessageingChannels();
                    Console.WriteLine("RabbitMQ initialized");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    await Task.Delay(500, stoppingToken);
                }
            }
        }

        private async Task DeclareMessageingChannels()
        {
            await _queueChannel.ExchangeDeclareAsync(_config.GetValue<string>("RabbitMQ:ExchangeName"), ExchangeType.Fanout);
            await _queueChannel.QueueDeclareAsync(_config.GetValue<string>("RabbitMQ:SearchQueueName"), exclusive: false);
            await _queueChannel.QueueDeclareAsync(_config.GetValue<string>("RabbitMQ:BookingQueueName"), exclusive: false);
            await _queueChannel.QueueBindAsync(_config.GetValue<string>("RabbitMQ:SearchQueueName"), _config.GetValue<string>("RabbitMQ:ExchangeName"), "");
            await _queueChannel.QueueBindAsync(_config.GetValue<string>("RabbitMQ:BookingQueueName"), _config.GetValue<string>("RabbitMQ:ExchangeName"), "");

            _isInitialized = true;
        }
    }
}
