﻿namespace EducateAPI.LoadGPTService.Interfaces
{
    public interface IRabbitMQPublisher<T>
    {
        Task PublishMessageAsync(T message, string queueName, string exchange);
    }
}
