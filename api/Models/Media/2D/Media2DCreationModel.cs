using System.ComponentModel.DataAnnotations;
using api.Enums.Media;
using Microsoft.AspNetCore.Http;

namespace api.Models.Media._2D
{
    public class Media2DCreationDto
    {
        [Required]
        public AspectRatio AspectRatio { get; set; }

        [Required]
        public IFormFile Texture { get; set; }

        [Required]
        public IFormFile NormalMap { get; set; }
    }
}