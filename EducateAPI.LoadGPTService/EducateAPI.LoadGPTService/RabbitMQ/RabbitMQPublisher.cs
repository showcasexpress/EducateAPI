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
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _env;
        public RabbitMQPublisher(IOptions<RabbitMQConfiguration> rabbitMQconfiguration, IConfiguration configuration, IHostEnvironment env)
        {
            _rabbitMQconfiguration = rabbitMQconfiguration.Value;
            _configuration = configuration;
            _env= env;
        }

        public async Task PublishMessageAsync(T message, string queueName)
        {
            try
            {
                string rabbitPassword;

                if (_env.IsDevelopment())
                    rabbitPassword = _configuration["RabbitMQ:Password"]??"";
                else
                    rabbitPassword = Environment.GetEnvironmentVariable("RabbitMqPassword") ??"";

                if (string.IsNullOrEmpty(rabbitPassword))
                    throw new InvalidOperationException("Invalid configuration, rabbitMQ password missing or empty.");

                var uri = $"amqp://{_rabbitMQconfiguration.UserName}:{rabbitPassword}@{_rabbitMQconfiguration.HostName}:5672";
                var factory = new ConnectionFactory
                {
                    Uri = new Uri(uri)
                    //HostName = _rabbitMQconfiguration.HostName,
                    //UserName = _rabbitMQconfiguration.UserName,
                    //Password = rabbitPassword
                };

                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();
                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                string messageJson = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(messageJson);

                await Task.Run(() => channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error publishing the message : {ex.Message}");
            }
        }
    }
}
