using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace api.Models.Advertisement
{
    public class AdvertisementCreationDto
    {
        [BindRequired]
        public Guid CampaignId { get; set; }
        
        public Guid MediaId { get; set; }
        
        [Required]
        public string Name { get; set; }

        [RangeAttribute(1, 120)]
        public int AgeMin { get; set; }
        
        [RangeAttribute(1, 120)]
        public int AgeMax { get; set; }
    }
}