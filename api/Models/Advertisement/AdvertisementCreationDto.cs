using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace api.Models.Advertisement
{
    public class AdvertisementCreationDto
    {
        [BindRequired]
        public Guid OrganizationId { get; set; }
        
        public IList<string> TagNames { get; set; }
        
        [Required, RangeAttribute(0, 120)]
        public int AgeMin { get; set; }
        
        [Required, RangeAttribute(0, 120)]
        public int AgeMax { get; set; }
    }
}