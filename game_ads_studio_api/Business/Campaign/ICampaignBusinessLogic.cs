using game_ads_studio_api.Models.Campaign;

namespace game_ads_studio_api.Business.Campaign
{
    internal interface ICampaignBusinessLogic
    {
        CampaignModel AddNewCampaign(CampaignCreationModel newCampaign);
        CampaignModel UpdateCampaignById(string id, CampaignUpdateModel updatedCampaign);
        int DeleteCampaignById(string id);
    }
}
