using EducateAPI.LoadGPTService.Interfaces;
using EducateAPI.LoadGPTService.RabbitMQ;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace EducateAPI.LoadGPTService.Services
{
    public class GPTService : BackgroundService
    {
        private readonly IRabbitMQConnectionFactory _factory;
        private readonly IRabbitMQPublisher<string> _publisher;
        private readonly IGPTClient _gPTClient;
        public GPTService(IRabbitMQConnectionFactory factory, IRabbitMQPublisher<string> publisher, IGPTClient gPTClient)
        {
            _factory = factory;
            _publisher = publisher;
            _gPTClient = gPTClient;
        }

        private async Task<IConnection> TryConnectRabbitMQAsync()
        {
            int retries = 5;
            while (retries > 0)
            {
                try
                {
                    var connection = _factory.CreateConnection();
                    return connection;
                }
                catch (Exception)
                {
                    retries--;
                    if (retries == 0) throw; // If no connection is made after retries, throw the exception
                    await Task.Delay(5000); // Wait for 1 second before retrying
                }
            }
            throw new Exception("Could not connect to RabbitMQ after several attempts.");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Setting up the GPT background service");

            var connection = await TryConnectRabbitMQAsync();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "post.exchange", type: ExchangeType.Direct);

            // Queues
            channel.QueueDeclare(queue: "post.request", durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueDeclare(queue: "post.generated", durable: false, exclusive: false, autoDelete: false, arguments: null);

            // Bind
            channel.QueueBind(queue: "post.request", exchange: "post.exchange", routingKey: "post.request");


            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var promptReceived = message;//JsonSerializer.Deserialize<string>(message);

                // if the request is not empty, call the GPT client to get a response.

                if (!string.IsNullOrEmpty(promptReceived))
                {
                    Console.WriteLine($"GPTService - Prompt received :{promptReceived}");
                    var gptResponse = await _gPTClient.GetResponseAsync(promptReceived);

                    Console.WriteLine($"GPTService - Response received :{gptResponse}");

                    // Send the response back to the queue.
                    await _publisher.PublishMessageAsync(gptResponse, "post.generated", "post.exchange");
                }
                else
                {
                    // Reject the invalid message
                    channel.BasicReject(deliveryTag: ea.DeliveryTag, requeue: false);
                }

            };

            channel.BasicConsume(queue: "post.request", autoAck: false, consumer: consumer);
            Console.WriteLine("GPT Service is running...");

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
