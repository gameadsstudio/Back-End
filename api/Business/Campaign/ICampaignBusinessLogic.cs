using System.Collections.Generic;
using api.Models.Campaign;

namespace api.Business.Campaign
{
    public interface ICampaignBusinessLogic
    {
        CampaignModel AddNewCampaign(CampaignCreationModel newCampaign);
        CampaignModel UpdateCampaignById(string id, CampaignUpdateModel updatedCampaign);
        int DeleteCampaignById(string id);
		IList<CampaignModel> GetOrganizationCampaigns(string id);
    }
}
