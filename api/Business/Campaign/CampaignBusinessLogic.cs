﻿using System;
using System.Collections.Generic;
using System.Net;
using AutoMapper;
using api.Business.Organization;
using api.Contexts;
using api.Errors;
using api.Models.Campaign;
using api.Repositories.Campaign;
using api.Helpers;
using api.Models.Advertisement;

namespace api.Business.Campaign
{
    public class CampaignBusinessLogic : ICampaignBusinessLogic
    {
        private readonly IMapper _mapper;

        private readonly IOrganizationBusinessLogic _organizationBusinessLogic;

        private readonly ICampaignRepository _repository;

        public CampaignBusinessLogic(ApiContext context, IMapper mapper,
            IOrganizationBusinessLogic organizationBusinessLogic)
        {
            _repository = new CampaignRepository(context);
            _mapper = mapper;
            _organizationBusinessLogic = organizationBusinessLogic;
        }

        public CampaignPublicDto AddNewCampaign(CampaignCreationDto newCampaign, ConnectedUser currentUser)
        {
            var campaign = _mapper.Map(newCampaign, new CampaignModel());
            var organization =
                _organizationBusinessLogic.GetOrganizationModelById(newCampaign.OrganizationId);

            if (!_organizationBusinessLogic.IsUserInOrganization(campaign.Organization.Id, currentUser.Id))
            {
                throw new CampaignInsufficientRightsError();
            }
            
            if (campaign.DateBegin >= campaign.DateEnd)
            {
                throw new CampaignStartAfterEndError();
            }

            campaign.Organization = organization;
            campaign.Advertisements = new List<AdvertisementModel>();
            return _mapper.Map(_repository.AddNewCampaign(campaign), new CampaignPublicDto());
        }

        public CampaignPublicDto UpdateCampaignById(Guid id, CampaignUpdateDto updatedCampaign,
            ConnectedUser currentUser)
        {
            var campaignMerge = _mapper.Map(updatedCampaign, _repository.GetCampaignById(id));

            if (!_organizationBusinessLogic.IsUserInOrganization(campaignMerge.Organization.Id, currentUser.Id))
            {
                throw new CampaignInsufficientRightsError();
            }

            if (campaignMerge.DateBegin >= campaignMerge.DateEnd)
            {
                throw new CampaignStartAfterEndError();
            }
            
            return _mapper.Map(_repository.UpdateCampaign(campaignMerge), new CampaignPublicDto());
        }

        public void DeleteCampaignById(Guid id, ConnectedUser currentUser)
        {
            var campaign = _repository.GetCampaignById(id);

            if (!_organizationBusinessLogic.IsUserInOrganization(campaign.Organization.Id, currentUser.Id))
            {
                throw new CampaignInsufficientRightsError();
            }

            _repository.DeleteCampaign(campaign);
        }

        public CampaignPublicDto GetCampaignById(Guid id, ConnectedUser currentUser)
        {
            var campaign = _repository.GetCampaignById(id);

            if (!_organizationBusinessLogic.IsUserInOrganization(campaign.Organization.Id, currentUser.Id))
            {
                throw new CampaignInsufficientRightsError();
            }

            return _mapper.Map(campaign, new CampaignPublicDto());
        }

        public (int page, int pageSize, int totalItemCount, IList<CampaignPublicDto> campaigns) GetCampaigns(PagingDto paging,
            CampaignFiltersDto filters, ConnectedUser currentUser)
        {
            if (!_organizationBusinessLogic.IsUserInOrganization(filters.OrganizationId, currentUser.Id))
            {
                throw new CampaignInsufficientRightsError();
            }

            paging = PagingHelper.Check(paging);
            var (campaigns, totalItemCount) = _repository.GetCampaigns(filters,
                (paging.Page - 1) * paging.PageSize, paging.PageSize);
            return (paging.Page, paging.PageSize, totalItemCount,
                _mapper.Map(campaigns, new List<CampaignPublicDto>()));
        }

        public CampaignModel GetCampaignModelById(Guid id)
        {
            var campaign = _repository.GetCampaignById(id);

            if (campaign == null)
            {
                throw new CampaignNotFoundError();
            }

            return campaign;
        }
    }
}