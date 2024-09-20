namespace EducateAPI.LoadGPTService.RabbitMQ
{
    public class RabbitMQConfiguration
    {
        public string? HostName { get; set; }
        public string? UserName { get; set; }
        public string? PassWord { get; set; }
    }

    // RabbitMQ Queue names
    public static class RabbitQueues
    {
        public const string StoriesQueue = "StoriesQueue";
        public const string QuizzQueue = "QuizzQueue";
    }
}
