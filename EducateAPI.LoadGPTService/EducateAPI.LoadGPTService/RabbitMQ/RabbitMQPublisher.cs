using EducateAPI.LoadGPTService.Interfaces;
using MassTransit.Configuration;
using RabbitMQ.Client;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace EducateAPI.LoadGPTService.RabbitMQ.Services
{
    public class RabbitMQPublisher<T> : IRabbitMQPublisher<T>
    {
        private readonly RabbitMQConfiguration _rabbitMQconfiguration;
        private readonly IConfiguration _config;
        public RabbitMQPublisher(IConfiguration config, IOptions<RabbitMQConfiguration> rabbitMQconfiguration)
        {
            _rabbitMQconfiguration = rabbitMQconfiguration.Value;
            _config = config;
        }

        public async Task PublishMessageAsync(T message, string queueName)
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbitMQconfiguration.HostName,
                UserName = _rabbitMQconfiguration.UserName,
                Password = _config["Rabbitmq:password"] //_rabbitMQconfiguration.PassWord
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            string messageJson = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(messageJson);
            
            await Task.Run(() => channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body));
        }
    }
}
