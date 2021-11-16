using System.ComponentModel.DataAnnotations;
using api.DataAnnotations;
using api.Enums.Organization;

namespace api.Models.Organization
{
    public class OrganizationCreationDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RequiredEnumAttribute]
        public OrganizationType Type { get; set; }

        public string LogoUrl { get; set; }

        public string WebsiteUrl { get; set; }
    }
}
