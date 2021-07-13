  
using System;
using System.Collections.Generic;
using System.Net;
using AutoMapper;
using api.Business.Organization;
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

        public CampaignBusinessLogic(ApiContext context, IMapper mapper, IOrganizationBusinessLogic organizationBusinessLogic)
        {
            _repository = new CampaignRepository(context);
            _mapper = mapper;
            _organizationBusinessLogic = organizationBusinessLogic;
        }

        public CampaignPublicDto AddNewCampaign(CampaignCreationDto newCampaign, ConnectedUser currentUser)
        {
            CampaignModel campaign = _mapper.Map(
                newCampaign,
                new CampaignModel()
            );
            var organization = _organizationBusinessLogic.GetOrganizationModelById(
                newCampaign.OrganizationId.ToString()
            );

            if (!_organizationBusinessLogic.IsUserInOrganization(campaign.Organization.Id, currentUser.Id)) {
                throw new ApiError(HttpStatusCode.Forbidden,
                    "Cannot create a campaign for an organization you're not part of");
            }
            campaign.Organization = organization;
            return _mapper.Map(
                _repository.AddNewCampaign(campaign),
                new CampaignPublicDto()
            );
        }

        public CampaignPublicDto UpdateCampaignById(Guid id, CampaignUpdateDto updatedCampaign, ConnectedUser currentUser)
        {
            var campaignMerge =_mapper.Map(
                updatedCampaign,
                _repository.GetCampaignById(id)
            );

            if (!_organizationBusinessLogic.IsUserInOrganization(campaignMerge.Organization.Id, currentUser.Id)) {
                throw new ApiError(HttpStatusCode.Forbidden,
                    "Cannot update a campaign for an organization you're not part of");
            }
            return _mapper.Map(
                _repository.UpdateCampaign(campaignMerge),
                new CampaignPublicDto()
            );
        }

        public void DeleteCampaignById(Guid id, ConnectedUser currentUser)
        {
            var campaign = _repository.GetCampaignById(id);

            if (!_organizationBusinessLogic.IsUserInOrganization(campaign.Organization.Id, currentUser.Id)) {
                throw new ApiError(HttpStatusCode.Forbidden,
                    "Cannot delete a campaign for an organization you're not part of");
            }
            _repository.DeleteCampaign(campaign);
        }

        public CampaignPublicDto GetCampaignById(Guid id, ConnectedUser currentUser)
        {
            var campaign = _repository.GetCampaignById(id);

            if (!_organizationBusinessLogic.IsUserInOrganization(campaign.Organization.Id, currentUser.Id)) {
                throw new ApiError(HttpStatusCode.Forbidden,
                    "Cannot get a campaign from an organization to which you don't belong.");
            }
            return _mapper.Map(campaign, new CampaignPublicDto());
        }

        public (int page, int pageSize, int maxPage, IList<CampaignPublicDto> campaigns) GetCampaigns(PagingDto paging, CampaignFiltersDto filters, ConnectedUser currentUser)
        {
            if (!_organizationBusinessLogic.IsUserInOrganization(filters.OrganizationId, currentUser.Id)) {
                throw new ApiError(HttpStatusCode.Forbidden,
                    "Cannot get a campaign from an organization to which you don't belong.");
            }
            paging = PagingHelper.Check(paging);
            var (campaigns, maxPage) = _repository.GetOrganizationCampaigns(
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