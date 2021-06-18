using System;
using System.ComponentModel.DataAnnotations;

namespace api.Models.Campaign
{
    public class CampaignDto
    {
        [Required]
        public string OrganizationId { get; set; }
    }
}
