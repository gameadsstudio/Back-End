using System;
using System.Collections.Generic;
using api.Helpers;
using api.Models.Campaign;

namespace api.Business.Campaign
{
    public interface ICampaignBusinessLogic
    {
        CampaignPublicDto AddNewCampaign(CampaignCreationDto newCampaign, ConnectedUser currentUser);

        CampaignPublicDto UpdateCampaignById(Guid id, CampaignUpdateDto updatedCampaign, ConnectedUser currentUser);

        int DeleteCampaignById(Guid id, ConnectedUser currentUser);

        CampaignPublicDto GetCampaignById(Guid id, ConnectedUser currentUser);

        (int page, int pageSize, int maxPage, IList<CampaignPublicDto> campaigns) GetCampaigns(PagingDto paging, CampaignFiltersDto filters, ConnectedUser currentUser);
    }
}
