using System;

namespace api.Models.Organization
{
    public class OrganizationPublicDto : IOrganizationDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string LogoUrl { get; set; }

        public string WebsiteUrl { get; set; }
    }
}
