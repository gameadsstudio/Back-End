using System.Collections.Generic;
using GAME_ADS_STUDIO_API.Models.Campaign;

namespace GAME_ADS_STUDIO_API.Repositories.Campaign
{
    public interface ICampaignRepository
    {
        int AddNewCampaign(CampaignModel Campaign);
        int UpdateCampaign(CampaignModel updatedCampaign, CampaignModel targetCampaign);
        int DeleteCampaign(CampaignModel Campaign);
    }
}
