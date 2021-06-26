using System;
using api.Enums.Media;

namespace api.Models.Media._2D
{
    public class Media2DPublicDto
    {
        public AspectRatio AspectRatio { get; set; }

        public Uri TextureLink { get; set; }

        public Uri NormalMapLink { get; set; }
    }
}