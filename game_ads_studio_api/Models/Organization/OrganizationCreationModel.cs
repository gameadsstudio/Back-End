using System.ComponentModel.DataAnnotations;

namespace game_ads_studio_api.Models.Organization
{
    public class OrganizationCreationModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string PrivateEmail { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
