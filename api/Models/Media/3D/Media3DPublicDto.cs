namespace api.Models.Media._3D
{
    public class Media3DPublicDto
    {
        public string ModelLink { get; set; }

        public string TextureLink { get; set; }

        public string NormalMapLink { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int Depth { get; set; }
    }
}