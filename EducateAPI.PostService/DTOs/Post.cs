using System.ComponentModel.DataAnnotations.Schema;

namespace EducateAPI.PostService.DTOs
{
    public class Post
    {
        public Guid PostId { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
        public int? InterestId { get; set; }

        [ForeignKey("InterestId")]
        public Interest? Interest { get; set; }
    }
}
