using System;
using game_ads_studio_api.Contexts;
using game_ads_studio_api.Models.Campaign;

namespace game_ads_studio_api.Repositories.Campaign
{
    public class CampaignRepository : ICampaignRepository
    {

        private readonly ApiContext _context;

        public CampaignRepository(ApiContext context)
        {
            _context = context;
        }

        public int AddNewCampaign(CampaignModel campaign)
        {
            throw new NotImplementedException();
        }

        public int DeleteCampaign(CampaignModel campaign)
        {
            throw new NotImplementedException();
        }

        public int UpdateCampaign(CampaignModel updatedCampaign, CampaignModel targetCampaign)
        {
            throw new NotImplementedException();
        }

    }
}
