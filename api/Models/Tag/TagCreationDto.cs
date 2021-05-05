using System.ComponentModel.DataAnnotations;

namespace api.Models.Tag
{
    public class TagCreationDto
    {
        [Required]
        [MinLength(2)]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}