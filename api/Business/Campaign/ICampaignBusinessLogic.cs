using api.Models.Campaign;

namespace api.Business.Campaign
{
    internal interface ICampaignBusinessLogic
    {
        CampaignModel AddNewCampaign(CampaignCreationModel newCampaign);
        CampaignModel UpdateCampaignById(string id, CampaignUpdateModel updatedCampaign);
        int DeleteCampaignById(string id);
    }
}
