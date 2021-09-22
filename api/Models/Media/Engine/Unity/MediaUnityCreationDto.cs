using api.Enums.Media;
using Microsoft.AspNetCore.Http;

namespace api.Models.Media.Engine.Unity
{
    public class MediaUnityCreationDto
    {
        public IFormFile AssetBundle { get; set; }

        public MediaStateEnum State { get; set; }

        public string StateMessage { get; set; }
    }
}