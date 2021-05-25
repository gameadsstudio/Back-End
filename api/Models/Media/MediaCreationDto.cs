using System.Collections.Generic;
using api.Enums.Media;
using Microsoft.AspNetCore.Http;

namespace api.Models.Media
{
    public class MediaCreationDto
    {
        public IFormFile Model { get; set; }

        public IFormFile Texture { get; set; }

        public IFormFile NormalMap { get; set; }

        public Type Type { get; set; }

        public AspectRatio AspectRatio { get; set; }

        public IList<string> TagName { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int Depth { get; set; }
    }
}