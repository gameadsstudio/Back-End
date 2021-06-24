using System;
using System.Collections.Generic;
using api.Configuration;
using api.Contexts;
using api.Models.Campaign;
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

        public CampaignPublicDto AddNewCampaign(CampaignCreationDto newCampaign)
        {
            var campaign = _mapper.Map(newCampaign, new CampaignModel());

            return _mapper.Map(
                _repository.AddNewCampaign(campaign),
                new CampaignPublicDto()
            );
        }

        public CampaignPublicDto UpdateCampaignById(Guid id, CampaignUpdateDto updatedCampaign)
        {
            var campaignMerge =_mapper.Map(
                updatedCampaign,
                _repository.GetCampaignById(id)
            );

            return _mapper.Map(
                _repository.UpdateCampaign(campaignMerge),
                new CampaignPublicDto()
            );
        }

        public int DeleteCampaignById(Guid id)
        {
            var campaign = _repository.GetCampaignById(id);

            return _repository.DeleteCampaign(campaign);
        }

        public CampaignPublicDto GetCampaignById(Guid id)
        {
            return _mapper.Map(
                _repository.GetCampaignById(id),
                new CampaignPublicDto()
            );
        }

        public IList<CampaignPublicDto> GetAll(CampaignFiltersDto filters)
        {
            return _mapper.Map(
                _repository.GetOrganizationCampaigns(filters.OrganizationId),
                new List<CampaignPublicDto>()
            );
        }
    }
}
