using System;
using System.Collections.Generic;
using api.Models.Campaign;

namespace api.Repositories.Campaign
{
    public interface ICampaignRepository
    {
        CampaignModel AddNewCampaign(CampaignModel campaign);
        CampaignModel UpdateCampaign(CampaignModel campaign);
        int DeleteCampaign(CampaignModel campaign);

        CampaignModel GetCampaignById(Guid id);

        (IList<CampaignModel> campaigns, int count) GetCampaigns(CampaignFiltersDto filters, int offset, int limit);
    }
}