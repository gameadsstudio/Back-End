using System;
using System.ComponentModel.DataAnnotations;

namespace api.Models.Advertisement
{
    public class AdvertisementCreationModel
    {
        [Required, RangeAttribute(0, 120)]
        public int AgeMin { get; set; }
        [Required, RangeAttribute(0, 120)]
        public int AgeMax { get; set; }
        [Required]
        public String Status { get; set; }
    }
}