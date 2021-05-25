using System;
using System.Collections.Generic;
using api.Models.Tag;
using api.Models.Version;
using api.Enums.Media;
using Type = api.Enums.Media.Type;

namespace api.Models.AdContainer
{
    public class AdContainerPublicDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public VersionPublicDto Version { get; set; }

        public IList<TagPublicDto> Tags { get; set; }

        public Type Type { get; set; }

        public AspectRatio AspectRatio { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int Depth { get; set; }

        public DateTimeOffset DateCreation { get; set; }

        public DateTimeOffset DateUpdate { get; set; }
    }
}