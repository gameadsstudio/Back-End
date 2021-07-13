using System;
using System.Collections.Generic;
using api.Configuration;
using api.Contexts;
using api.Models.Campaign;
using api.Models.Organization;
using api.Repositories.Campaign;
using api.Helpers;
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

		public CampaignModel GetCampaignById(string id)
		{
			return _repository.GetCampaignById(Guid.Parse(id));
		}

        public (int page, int pageSize, int maxPage, IList<CampaignPublicDto> campaigns) GetCampaigns(PagingDto paging, CampaignFiltersDto filters)
        {
            IList<CampaignModel> campaigns = null;
            int maxPage = 0;

            paging = PagingHelper.Check(paging);
            (campaigns, maxPage) = _repository.GetOrganizationCampaigns(
                filters.OrganizationId,
                (paging.Page - 1) * paging.PageSize,
                paging.PageSize
            );
            return (
                paging.Page,
                paging.PageSize,
                (maxPage / paging.PageSize + 1),
                _mapper.Map(campaigns, new List<CampaignPublicDto>())
            );
        }
    }
}
