using System;

namespace api.Models.Organization
{
    public class OrganizationPublicDto : IOrganizationDto
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public string PublicEmail { get; set; }
        
        public string Localization { get; set; }
        
        public string LogoUrl { get; set; }
        
        public string WebsiteUrl { get; set; }
    }
}