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
                OrganizationId = Guid.Parse(newCampaign.OrganizationId),
                Name = newCampaign.Name,
                AgeMin = newCampaign.AgeMin,
                AgeMax = newCampaign.AgeMax,
                Type = newCampaign.Type,
                Status = newCampaign.Status,
                DateBegin = newCampaign.DateBegin,
                DateEnd = newCampaign.DateEnd,
                DateCreation = DateTime.Now,
                DateUpdate = DateTime.Now
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
