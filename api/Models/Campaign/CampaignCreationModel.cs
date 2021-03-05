using System;
using System.ComponentModel.DataAnnotations;

namespace api.Models.Campaign
{
    public class CampaignCreationModel
    {
        [Required]
        public string OrganizationId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string AgeMin { get; set; }

        [Required]
        public string AgeMax { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public DateTimeOffset DateBegin { get; set; }

        [Required]
        public DateTimeOffset DateEnd { get; set; }
    }
}
