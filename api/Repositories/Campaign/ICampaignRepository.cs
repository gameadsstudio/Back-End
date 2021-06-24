﻿using System;
using System.Collections.Generic;
using api.Models.Campaign;

namespace api.Repositories.Campaign
{
    public interface ICampaignRepository
    {
        CampaignModel AddNewCampaign(CampaignModel campaign);

        CampaignModel UpdateCampaign(CampaignModel campaign);

        int DeleteCampaign(CampaignModel campaign);

        IList<CampaignModel> GetOrganizationCampaigns(Guid id);

        CampaignModel GetCampaignById(Guid id);
    }
}
