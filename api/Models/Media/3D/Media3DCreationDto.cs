using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace api.Models.Media._3D
{
    public class Media3DCreationDto
    {
        [Required]
        public IFormFile Texture { get; set; }

        [Required]
        public IFormFile NormalMap { get; set; }

        [Required]
        public IFormFile Model { get; set; }

        [Required]
        public int Width { get; set; }

        [Required]
        public int Height { get; set; }

        [Required]
        public int Depth { get; set; }
    }
}