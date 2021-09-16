using System.ComponentModel.DataAnnotations;

namespace api.Models.Post
{
    public class PostCreationDto
    {
        [Required]
        public string Name { get; set; }

        public string Category { get; set; }

        public string Content { get; set; }
    }
}
