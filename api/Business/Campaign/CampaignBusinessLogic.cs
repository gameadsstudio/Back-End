using System;
using System.Collections.Generic;
using api.Configuration;
using api.Contexts;
using api.Models.Campaign;
using api.Models.Organization;
using api.Repositories.Campaign;
using Microsoft.Extensions.Options;
using AutoMapper;

namespace api.Business.Campaign
{
    public class CampaignBusinessLogic : ICampaignBusinessLogic
    {
		private readonly IMapper _mapper;
        private readonly ICampaignRepository _repository;

        public CampaignBusinessLogic(ApiContext context, IOptions<AppSettings> appSettings, IMapper mapper)
        {
            _repository = new CampaignRepository(context);
			_mapper = mapper;
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
                Organization = new OrganizationModel
                {
                    Id = Guid.Parse(newCampaign.OrganizationId)
                }
            };

			return _repository.AddNewCampaign(campaign);
        }

        public CampaignModel UpdateCampaignById(string id, CampaignUpdateModel updatedCampaign)
        {
			var campaignMerge =_mapper.Map<CampaignUpdateModel, CampaignModel>(
				updatedCampaign,
				_repository.GetCampaignById(Guid.Parse(id))
			);

            return _repository.UpdateCampaign(campaignMerge);
        }

        public int DeleteCampaignById(string id)
        {
			var campaign = _repository.GetCampaignById(Guid.Parse(id));

			_repository.DeleteCampaign(campaign);
			return 0;
        }

		public IList<CampaignModel> GetOrganizationCampaigns(string id)
		{
			return _repository.GetOrganizationCampaigns(Guid.Parse(id));
		}
    }
}
