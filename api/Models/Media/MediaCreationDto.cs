using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using api.Enums.Media;
using Microsoft.AspNetCore.Http;
using Type = api.Enums.Media.Type;

namespace api.Models.Media
{
    public class MediaCreationDto
    {
        [Required]
        public Type Type { get; set; }

        public string Name { get; set; }

        [Required]
        public IList<string> TagName { get; set; }

        [Required]
        public string OrganizationId { get; set; }

        public AspectRatio AspectRatio { get; set; }

        public IFormFile Texture { get; set; }

        public IFormFile NormalMap { get; set; }

        public IFormFile Model { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int Depth { get; set; }
    }
}