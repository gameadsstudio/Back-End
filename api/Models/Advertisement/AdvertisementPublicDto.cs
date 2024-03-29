using System;
using api.Models.Media;

namespace api.Models.Advertisement
{
    public class AdvertisementPublicDto
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public int AgeMin { get; set; }
        
        public int AgeMax { get; set; }
        
        public MediaPublicDto Media { get; set; }

        public DateTimeOffset DateCreation { get; set; }
        
        public DateTimeOffset DateUpdate { get; set; }
    }
}