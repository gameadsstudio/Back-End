using GAME_ADS_STUDIO_API.Models.Campaign;
using GAME_ADS_STUDIO_API.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAME_ADS_STUDIO_API.Business.Campaign
{
    interface ICampaignBusinessLogic
    {
        CampaignModel AddNewCampaign(CampaignCreationModel newCampaign);
        CampaignPublicModel[] GetCampaigns(int offset, int limit);
        CampaignModel GetCampaignById(int id);
        int UpdateCampaignById(int id, CampaignUpdateModel updatedCampaign);
        int DeleteCampaignById(int id);
    }
}