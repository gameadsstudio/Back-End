using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace api.Models.Advertisement
{
    public class AdvertisementUpdateDto
    {
        public string Name { get; set; }

        [RangeAttribute(0, 120)]
        public int AgeMin { get; set; }
        
        [RangeAttribute(0, 120)]
        public int AgeMax { get; set; }
        
        public string MediaId { get; set; }
    }
}