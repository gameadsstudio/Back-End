using Microsoft.AspNetCore.Http;

namespace api.Models.Media.Engine.Unity
{
    public class MediaUnityCreationDto
    {
        public IFormFile AssetBundle { get; set; }

        public MediaState State { get; set; }
    }
}