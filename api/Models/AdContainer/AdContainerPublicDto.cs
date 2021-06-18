using System;
using System.Collections.Generic;
using api.Enums.AdContainer;
using api.Models.Tag;
using api.Models.Version;

namespace api.Models.AdContainer
{
    public class AdContainerPublicDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public VersionPublicDto Version { get; set; }

        public IList<TagPublicDto> Tags { get; set; }

        public AdContainerType Type { get; set; }

        public AdContainerAspectRatio AspectRatio { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int Depth { get; set; }

        public DateTimeOffset DateCreation { get; set; }

        public DateTimeOffset DateUpdate { get; set; }
    }
}