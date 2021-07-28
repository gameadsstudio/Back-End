using System;
using System.Collections.Generic;
using api.Enums.Campaign;
using api.Models.Advertisement;

namespace api.Models.Campaign
{
    public class CampaignPublicDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string AgeMin { get; set; }

        public string AgeMax { get; set; }

        public string Type { get; set; }
        
        public IList<AdvertisementPublicDto> Advertisements { get; set; }

        public CampaignStatus Status { get; set; }

        public DateTimeOffset DateBegin { get; set; }

        public DateTimeOffset DateEnd { get; set; }

        public DateTimeOffset DateCreation { get; set; }

        public DateTimeOffset DateUpdate { get; set; }
    }
}