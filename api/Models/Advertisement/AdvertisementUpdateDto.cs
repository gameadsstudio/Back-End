using System.ComponentModel.DataAnnotations;

namespace api.Models.Advertisement
{
    public class AdvertisementUpdateDto
    {
        [RangeAttribute(0, 120)]
        public int AgeMin { get; set; }
        
        [RangeAttribute(0, 120)]
        public int AgeMax { get; set; }
    }
}