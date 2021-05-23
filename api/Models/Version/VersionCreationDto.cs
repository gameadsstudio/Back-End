using System.ComponentModel.DataAnnotations;

namespace api.Models.Version
{
    public class VersionCreationDto
    {
        [Required]
        public string GameId { get; set; }
        
        [Required]
        public string Name { get; set; }
    }
}