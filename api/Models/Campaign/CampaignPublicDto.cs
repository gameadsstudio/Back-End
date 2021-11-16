using System;
using System.Collections.Generic;
using api.Models.Advertisement;

namespace api.Models.Campaign
{
    public class CampaignPublicDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int AgeMin { get; set; }

        public int AgeMax { get; set; }
        
        public DateTimeOffset DateBegin { get; set; }
        
        public DateTimeOffset DateEnd { get; set; }

        public IList<AdvertisementPublicDto> Advertisements { get; set; }

        public DateTimeOffset DateCreation { get; set; }

        public DateTimeOffset DateUpdate { get; set; }
    }
}