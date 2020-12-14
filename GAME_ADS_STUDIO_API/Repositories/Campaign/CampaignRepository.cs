using System;
using System.Collections.Generic;
using GAME_ADS_STUDIO_API.Contexts;
using GAME_ADS_STUDIO_API.Models.Campaign;

namespace GAME_ADS_STUDIO_API.Repositories.Campaign
{
    public class CampaignRepository : ICampaignRepository
    {

        private readonly ApiContext _context;

        public CampaignRepository(ApiContext context)
        {
            _context = context;
        }

        public int AddNewCampaign(CampaignModel Campaign)
        {
            throw new NotImplementedException();
        }

        public int DeleteCampaign(CampaignModel Campaign)
        {
            throw new NotImplementedException();
        }

        public int UpdateCampaign(CampaignModel updatedCampaign, CampaignModel targetCampaign)
        {
            throw new NotImplementedException();
        }

    }
}
