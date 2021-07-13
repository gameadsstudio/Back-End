using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace api.Models.Campaign
{
    public class CampaignCreationDto
    {
        [BindRequired]
        public Guid OrganizationId { get; set; }

        [Required]
        public string Name { get; set; }

        public string AgeMin { get; set; }

        public string AgeMax { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }

        public DateTimeOffset DateBegin { get; set; }

        public DateTimeOffset DateEnd { get; set; }
    }
}
