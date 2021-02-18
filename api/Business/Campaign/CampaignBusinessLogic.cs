using System;
using api.Configuration;
using api.Contexts;
using api.Models.Campaign;
using api.Repositories.Campaign;
using Microsoft.Extensions.Options;

namespace api.Business.Campaign
{
    public class CampaignBusinessLogic : ICampaignBusinessLogic
    {
        private readonly ICampaignRepository _repository;
        private readonly AppSettings _appSettings;

        public CampaignBusinessLogic(ApiContext context, IOptions<AppSettings> appSettings)
        {
            _repository = new CampaignRepository(context);
            _appSettings = appSettings.Value;
        }

        public CampaignModel AddNewCampaign(CampaignCreationModel newCampaign)
        {
            var campaign = new CampaignModel
            {
                OrgId = newCampaign.OrgId,
                CpgName = newCampaign.CpgName,
                CpgAgeMin = newCampaign.CpgAgeMin,
                CpgAgeMax = newCampaign.CpgAgeMax,
                CpgType = newCampaign.CpgType,
                CpgStatus = newCampaign.CpgStatus,
                CpgDateBegin = newCampaign.CpgDateBegin,
                CpgDateEnd = newCampaign.CpgDateEnd,
                CpgDateCreation = DateTime.Now,
                CpgDateUpdate = DateTime.Now
            };

            throw new NotImplementedException();
        }

        public CampaignModel UpdateCampaignById(string id, CampaignUpdateModel updatedCampaign)
        {
            throw new NotImplementedException();
        }

        public int DeleteCampaignById(string id)
        {
            throw new NotImplementedException();
        }
    }
}
