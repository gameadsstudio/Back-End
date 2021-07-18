using System.ComponentModel.DataAnnotations;

namespace api.Models.Advertisement
{
    public class AdvertisementCreationDto
    {
        [Required, RangeAttribute(0, 120)]
        public int AgeMin { get; set; }
        
        [Required, RangeAttribute(0, 120)]
        public int AgeMax { get; set; }
    }
}