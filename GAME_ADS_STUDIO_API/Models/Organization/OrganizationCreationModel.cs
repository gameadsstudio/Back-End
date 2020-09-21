using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GAME_ADS_STUDIO_API.Models.Organization
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
