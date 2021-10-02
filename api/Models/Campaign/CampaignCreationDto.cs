using System;
using System.ComponentModel.DataAnnotations;
using api.Enums.Campaign;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace api.Models.Campaign
{
    public class CampaignCreationDto
    {
        [BindRequired]
        public Guid OrganizationId { get; set; }

        [Required]
        public string Name { get; set; }

        public int AgeMin { get; set; }

        public int AgeMax { get; set; }

        public CampaignStatus Status { get; set; }

        public DateTimeOffset DateBegin { get; set; }

        public DateTimeOffset DateEnd { get; set; }
    }
}
