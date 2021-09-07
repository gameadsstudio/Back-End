using System.ComponentModel.DataAnnotations;

namespace api.Models.Organization
{
    public class OrganizationUpdateDto
    {
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Localization { get; set; }
        
        public string LogoUrl { get; set; }
        
        public string WebsiteUrl { get; set; }
    }
}
