namespace EducateAPI.LoadGPTService.DTOs
{
    public class QuizDTO
    {
        public Guid Id { get; set; }
        public string Question { get; set; }
        public List<string> Choices { get; set; }
        public List<string> CorrectAnswersIndexes { get; set; }
        public bool IsMultipleChoices { get; set; } 
    }
}
