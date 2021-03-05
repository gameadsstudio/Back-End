using System;
using api.Contexts;
using api.Models.Campaign;

namespace api.Repositories.Campaign
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
