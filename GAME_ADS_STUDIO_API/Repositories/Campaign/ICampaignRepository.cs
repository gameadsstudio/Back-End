using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GAME_ADS_STUDIO_API.Models.Campaign;

namespace GAME_ADS_STUDIO_API.Repositories.Campaign
{
    interface ICampaignRepository
    {
        int AddNewCampaign(CampaignModel Campaign);
        CampaignPublicModel[] GetCampaigns(int offset, int limit);
        CampaignModel GetCampaignById(int id);
        int UpdateCampaign(CampaignUpdateModel updatedCampaign, CampaignModel targetCampaign);
        int DeleteCampaign(CampaignModel Campaign);
    }
}
