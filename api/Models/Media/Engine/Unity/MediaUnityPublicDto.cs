using System;
using api.Enums.Media;

namespace api.Models.Media.Engine.Unity
{
    public class MediaUnityPublicDto
    {
        public Uri AssetBundleLink { get; set; }

        public MediaStateEnum State { get; set; }

        public string StateMessage { get; set; }
    }
}