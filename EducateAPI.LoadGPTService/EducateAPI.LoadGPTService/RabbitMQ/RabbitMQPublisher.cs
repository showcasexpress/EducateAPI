using EducateAPI.LoadGPTService.Interfaces;
using MassTransit.Configuration;
using RabbitMQ.Client;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Connections;

namespace EducateAPI.LoadGPTService.RabbitMQ.Services
{
    public class RabbitMQPublisher<T> : IRabbitMQPublisher<T>
    {

        private readonly IRabbitMQConnectionFactory _factory;
        public RabbitMQPublisher(IRabbitMQConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task PublishMessageAsync(T message, string queueName)
        {
            try
            {
                var connection = _factory.CreateConnection();
                using var channel = connection.CreateModel();
                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                string messageJson = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(messageJson);

                await Task.Run(() => channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body));

                Console.WriteLine(" [x] Sent {0}", message);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error publishing the message : {ex.Message}");
            }
        }
    }
}
