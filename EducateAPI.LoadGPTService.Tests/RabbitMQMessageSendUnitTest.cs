using EducateAPI.LoadGPTService.Interfaces;
using EducateAPI.LoadGPTService.RabbitMQ.Services;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace EducateAPI.LoadGPTService.Tests
{
    public class RabbitMQMessageSendUnitTest
    {
        private readonly Mock<IRabbitMQConnectionFactory> _mockFactory;
        private readonly Mock<IConnection> _mockConnection;
        private readonly Mock<IModel> _mockChannel;
        public RabbitMQMessageSendUnitTest()
        {
            _mockConnection= new Mock<IConnection>();
            _mockChannel = new Mock<IModel>();
            _mockFactory= new Mock<IRabbitMQConnectionFactory>();
        }

        [Fact]
        public async Task Publish_SendMessageToQueue()
        {
            // Arrange
            var publisher = new RabbitMQPublisher<string>(_mockFactory.Object);
            var testMessage = "Hi there!";
            var queueName = "OK";

            // Setup the mocks
            _mockFactory.Setup(f => f.CreateConnection()).Returns(_mockConnection.Object);
            _mockConnection.Setup(c => c.CreateModel()).Returns(_mockChannel.Object);

            _mockChannel.Setup(c => c.QueueDeclare(queueName, false, false, false, null)).Verifiable();
            _mockChannel.Setup(c => c.BasicPublish(It.IsAny<string>(), queueName, false, null, null)).Verifiable();

            // Act
            await publisher.PublishMessageAsync(testMessage, queueName);

            // Assert
            _mockChannel.Verify(c => c.QueueDeclare(queueName, false, false, false, null), Times.Once);
        }
    }
}
