using EducateAPI.LoadGPTService.Interfaces;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace EducateAPI.LoadGPTService.RabbitMQ
{
    public class RabbitMQConnection : IRabbitMQConnectionFactory
    {
        private readonly IConfiguration _configuration;
        private readonly RabbitMQConfiguration _rabbitMQconfiguration;
        private readonly IHostEnvironment _env;

        public RabbitMQConnection(IOptions<RabbitMQConfiguration> rabbitMQconfiguration, IConfiguration configuration, IHostEnvironment env)
        {
            _configuration = configuration;
            _rabbitMQconfiguration = rabbitMQconfiguration.Value;
            _env = env;
        }

        public IConnection CreateConnection()
        {
            string rabbitPassword = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "";

            if (string.IsNullOrEmpty(rabbitPassword))
                throw new InvalidOperationException("Invalid configuration, rabbitMQ password missing or empty.");

            var uri = $"amqp://{_rabbitMQconfiguration.UserName}:{rabbitPassword}@{_rabbitMQconfiguration.HostName}:5672";
            var factory = new ConnectionFactory
            {
                Uri = new Uri(uri)
            };

            using var connection = factory.CreateConnection();
            return factory.CreateConnection();
        }
    }
}
