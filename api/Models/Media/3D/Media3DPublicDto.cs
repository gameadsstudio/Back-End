using System;

namespace api.Models.Media._3D
{
    public class Media3DPublicDto
    {
        public Uri ModelLink { get; set; }

        public Uri TextureLink { get; set; }

        public Uri NormalMapLink { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int Depth { get; set; }
    }
}