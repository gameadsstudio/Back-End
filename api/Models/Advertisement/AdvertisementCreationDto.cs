using System.ComponentModel.DataAnnotations;
using api.DataAnnotations;
using api.Enums;
using api.Enums.Advertisement;

namespace api.Models.Advertisement
{
    public class AdvertisementCreationDto
    {
        [Required, RangeAttribute(0, 120)]
        public int AgeMin { get; set; }
        
        [Required, RangeAttribute(0, 120)]
        public int AgeMax { get; set; }
        
        [RequiredEnumAttribute]
        public AdvertisementStatus Status { get; set; }
    }
}