using System;
using System.Collections.Generic;
using api.Models.Organization;
using api.Models.Tag;

namespace api.Models.Media
{
    public class MediaPublicDto
    {
        public Guid Id { get; set; }

        public OrganizationPublicDto Organization { get; set; }

        public IList<TagPublicDto> Tags { get; set; }

        public Type Type { get; set; }

        public object Media { get; set; }

        public DateTimeOffset DateCreation { get; set; }

        public DateTimeOffset DateUpdate { get; set; }
    }
}