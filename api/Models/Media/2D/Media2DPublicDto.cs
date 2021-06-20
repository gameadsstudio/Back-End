using api.Enums.Media;

namespace api.Models.Media._2D
{
    public class Media2DPublicDto
    {
        public AspectRatio AspectRatio { get; set; }

        public string TextureLink { get; set; }

        public string NormalMapLink { get; set; }
    }
}