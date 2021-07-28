using System;
using System.Collections;
using System.Collections.Generic;
using api.Models.Campaign;
using api.Models.Tag;

namespace api.Models.Advertisement
{
    public class AdvertisementPublicDto
    {
        public Guid Id { get; set; }

        public int AgeMin { get; set; }
        
        public int AgeMax { get; set; }
        
        public IList<CampaignPublicDto> Campaigns { get; set; }
        
        public IList<TagPublicDto> Tags { get; set; }

        public DateTimeOffset DateCreation { get; set; }
        
        public DateTimeOffset DateUpdate { get; set; }
    }
}