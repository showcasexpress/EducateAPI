using RabbitMQ.Client;

namespace EducateAPI.LoadGPTService.Interfaces
{
    public interface IRabbitMQConnectionFactory
    {
        IConnection CreateConnection();
    }
}
