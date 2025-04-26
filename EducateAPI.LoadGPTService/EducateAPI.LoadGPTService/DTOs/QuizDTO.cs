namespace EducateAPI.LoadGPTService.DTOs
{
    public class QuizDTO
    {
        public Guid Id { get; set; }
        public required string Question { get; set; }
        public required List<string> Choices { get; set; }
        public required List<string>? CorrectAnswersIndexes { get; set; }
        public required bool IsMultipleChoices { get; set; } 
    }
}
