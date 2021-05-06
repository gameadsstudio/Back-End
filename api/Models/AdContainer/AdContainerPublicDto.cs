using System;
using System.Collections.Generic;
using api.Enums.AdContainer;
using api.Models.Organization;
using api.Models.Tag;

namespace api.Models.AdContainer
{
    public class AdContainerPublicDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        /*
          Todo: implement when versions are done
        public VersionModel Version { get; set; }
        */

        public OrganizationPublicDto Organization { get; set; }

        public ICollection<TagPublicDto> Tags { get; set; }

        public AdContainerType Type { get; set; }

        public AdContainerAspectRatio AspectRatio { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int Depth { get; set; }

        public DateTimeOffset DateCreation { get; set; }

        public DateTimeOffset DateUpdate { get; set; }
    }
}