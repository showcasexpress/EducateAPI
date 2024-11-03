using EducateAPI.LoadGPTService.Interfaces;
using EducateAPI.LoadGPTService.RabbitMQ;
using EducateAPI.LoadGPTService.RabbitMQ.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EducateAPI.LoadGPTService.Controllers
{
    [ApiController]
    [Route("api/v1/message")]
    public class MessagesController : ControllerBase
    {
        private readonly IRabbitMQPublisher<string> _rabbitMQPublisher;
        public MessagesController(IRabbitMQPublisher<string> rabbitMQPublisher)
        {
            _rabbitMQPublisher= rabbitMQPublisher;
        }


        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public void PostMessage([FromBody] string value)
        {
            Console.WriteLine(value);
            _rabbitMQPublisher.PublishMessageAsync(value, RabbitQueues.StoriesQueue);
        }


    }
}
