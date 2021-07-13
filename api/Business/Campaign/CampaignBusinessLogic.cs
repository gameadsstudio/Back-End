using System;
using api.Configuration;
using api.Contexts;
using api.Models.Campaign;
using api.Models.Organization;
using api.Repositories.Campaign;
using Microsoft.Extensions.Options;

namespace api.Business.Campaign
{
    public class CampaignBusinessLogic : ICampaignBusinessLogic
    {
        private readonly ICampaignRepository _repository;

        public CampaignBusinessLogic(ApiContext context, IOptions<AppSettings> appSettings)
        {
            _repository = new CampaignRepository(context);
        }

        public CampaignModel AddNewCampaign(CampaignCreationModel newCampaign)
        {
            var campaign = new CampaignModel
            {
                Name = newCampaign.Name,
                AgeMin = newCampaign.AgeMin,
                AgeMax = newCampaign.AgeMax,
                Type = newCampaign.Type,
                Status = newCampaign.Status,
                DateBegin = newCampaign.DateBegin,
                DateEnd = newCampaign.DateEnd,
                DateCreation = DateTime.Now,
                DateUpdate = DateTime.Now,
                // TODO : fetch from org BL then add organization to this model
                Organization = new OrganizationModel
                {
                    Id = Guid.Parse(newCampaign.OrganizationId)
                }
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
