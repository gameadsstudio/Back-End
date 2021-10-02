using System;
using api.Enums.Campaign;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace api.Models.Campaign
{
    public class CampaignFiltersDto
    {
        [BindRequired]
        public Guid OrganizationId { get; set; }
        
        public CampaignStatus Status { get; set; }
    }
}