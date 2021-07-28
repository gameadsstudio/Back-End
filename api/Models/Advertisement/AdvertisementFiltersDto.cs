using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace api.Models.Advertisement
{
    public class AdvertisementFiltersDto
    {
        [BindRequired]
        public Guid OrganizationId { get; set; }
        
        public Guid CampaignId { get; set; }
    }
}