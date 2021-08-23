using System;
using System.Collections.Generic;
using api.Enums.Media;
using api.Models.Organization;
using api.Models.Tag;
using Type = api.Enums.Media.Type;

namespace api.Models.Media
{
    public class MediaPublicDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public OrganizationPublicDto Organization { get; set; }

        public IList<TagPublicDto> Tags { get; set; }

        public Type Type { get; set; }

        public MediaStateEnum State { get; set; }

        public string StateMessage { get; set; }

        public object Media { get; set; }

        public DateTimeOffset DateCreation { get; set; }

        public DateTimeOffset DateUpdate { get; set; }
    }
}