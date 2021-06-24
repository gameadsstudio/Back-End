using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace api.Models.Campaign
{
    public class CampaignFiltersDto
    {
        [BindRequired]
        public Guid OrganizationId { get; set; }
    }
}
