using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Extensions.Options;
using AutoMapper;
using api.Business.Organization;
using api.Configuration;
using api.Contexts;
using api.Errors;
using api.Models.Campaign;
using api.Repositories.Campaign;
using api.Helpers;

namespace api.Business.Campaign
{
    public class CampaignBusinessLogic : ICampaignBusinessLogic
    {
        private readonly IMapper _mapper;

        private readonly IOrganizationBusinessLogic _organizationBusinessLogic;

        private readonly ICampaignRepository _repository;

        public CampaignBusinessLogic(ApiContext context, IOptions<AppSettings> appSettings, IMapper mapper, IOrganizationBusinessLogic organizationBusinessLogic)
        {
            _repository = new CampaignRepository(context);
            _mapper = mapper;
            _organizationBusinessLogic = organizationBusinessLogic;
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

        public CampaignPublicDto GetCampaignById(Guid id, ConnectedUser currentUser)
        {
            var campaign = _repository.GetCampaignById(id);

            /*if (!_organizationBusinessLogic.IsUserInOrganization(campaign.Organization.Id, currentUser.Id)) {
                throw new ApiError(HttpStatusCode.Forbidden,
                    "Cannot get a campaign from an organization to which you don't belong.");
            }*/
            return _mapper.Map(campaign, new CampaignPublicDto());
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
