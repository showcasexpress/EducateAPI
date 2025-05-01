using System.ComponentModel.DataAnnotations;

namespace EducateAPI.PostService.DTOs
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}
