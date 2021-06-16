using System.ComponentModel.DataAnnotations;
using api.DataAnnotations;
using api.Enums;

namespace api.Models.Organization
{
    public class OrganizationCreationDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string PrivateEmail { get; set; }

        [RequiredEnumAttribute]
        public OrganizationType Type { get; set; }
    }
}
