using api.Models.Campaign;

namespace api.Repositories.Campaign
{
    public interface ICampaignRepository
    {
        int AddNewCampaign(CampaignModel campaign);
        int UpdateCampaign(CampaignModel updatedCampaign, CampaignModel targetCampaign);
        int DeleteCampaign(CampaignModel campaign);
    }
}
