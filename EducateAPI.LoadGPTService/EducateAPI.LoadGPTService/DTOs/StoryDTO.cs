namespace EducateAPI.LoadGPTService.DTOs
{
    public class StoryDTO
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public  required string Text { get; set; }
    }
}
