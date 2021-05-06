using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using api.Business.Organization;
using api.Business.Tag;
using api.Contexts;
using api.Errors;
using api.Helpers;
using api.Models.AdContainer;
using api.Models.Tag;
using api.Repositories.AdContainer;
using AutoMapper;

namespace api.Business.AdContainer
{
    public class AdContainerBusinessLogic : IAdContainerBusinessLogic

    {
        private readonly IMapper _mapper;
        private readonly IAdContainerRepository _repository;

        private readonly ITagBusinessLogic _tagBusinessLogic;
        private readonly IOrganizationBusinessLogic _organizationBusinessLogic;

        public AdContainerBusinessLogic(
            ApiContext context,
            IMapper mapper,
            ITagBusinessLogic tagBusinessLogic,
            IOrganizationBusinessLogic organizationBusinessLogic)
        {
            _repository = new AdContainerRepository(context);
            _tagBusinessLogic = tagBusinessLogic;
            _organizationBusinessLogic = organizationBusinessLogic;
            _mapper = mapper;
        }

        public AdContainerPublicDto GetAdContainerById(string id, Claim currentUser)
        {
            // Todo : check if user is admin
            var adContainer = GetAdContainerModelById(id);
            if (!_organizationBusinessLogic.IsUserInOrganization(adContainer.Organization.Id.ToString(), currentUser.Value))
            {
                throw new ApiError(HttpStatusCode.Forbidden, "Cannot get the ad container of an organization you're not part of");
            }
            return _mapper.Map(adContainer, new AdContainerPublicDto());
        }

        private AdContainerModel GetAdContainerModelById(string id)
        {
            return _repository.GetAdContainerById(GuidHelper.StringToGuidConverter(id)) ??
                   throw new ApiError(HttpStatusCode.NotFound, $"Could not find ad container with Id: {id}");
        }

        public (int page, int pageSize, int maxPage, List<AdContainerPublicDto> adContainers) GetAdContainers(
            PagingDto paging,
            string orgId, Claim currentUser)
        {
            // Todo : check if the user is admin

            paging = PagingHelper.Check(paging);
            var (adContainers, maxPage) = _repository.GetAdContainersByOrganizationId(
                (paging.Page - 1) * paging.PageSize,
                paging.PageSize,
                GuidHelper.StringToGuidConverter(orgId), GuidHelper.StringToGuidConverter(currentUser.Value));
            return (paging.Page, paging.PageSize, (maxPage / paging.PageSize + 1),
                _mapper.Map(adContainers, new List<AdContainerPublicDto>()));
        }

        public AdContainerPublicDto AddNewAdContainer(AdContainerCreationDto newAdContainer, Claim currentUser)
        {
            // Todo : check if user is admin

            if (!_organizationBusinessLogic.IsUserInOrganization(newAdContainer.OrgId, currentUser.Value))
            {
                throw new ApiError(HttpStatusCode.Forbidden, "Cannot create an ad container for an organization you're not part of");
            }
            var adContainer = _mapper.Map(newAdContainer, new AdContainerModel());
            adContainer.Tags = ResolveTags(newAdContainer.TagNames);
            /*
             * Todo : Add version to model
             */
            adContainer.Organization = _organizationBusinessLogic.GetOrganizationModelById(newAdContainer.OrgId);
            return _mapper.Map(_repository.AddNewAdContainer(adContainer), new AdContainerPublicDto());
        }

        public AdContainerPublicDto UpdateAdContainerById(string id, AdContainerUpdateDto updatedAdContainer,
            Claim currentUser)
        {
            // Todo : check if user is admin

            var adContainer = GetAdContainerModelById(id);
            if (!_organizationBusinessLogic.IsUserInOrganization(adContainer.Organization.Id.ToString(), currentUser.Value))
            {
                throw new ApiError(HttpStatusCode.Forbidden, "Cannot get the ad container of an organization you're not part of");
            }
            var updated = _mapper.Map(updatedAdContainer, adContainer);
            if (updatedAdContainer.TagNames != null)
            {
                updated.Tags = ResolveTags(updatedAdContainer.TagNames);
            }

            return _mapper.Map(_repository.UpdateAdContainer(updated), new AdContainerPublicDto());
        }

        public void DeleteAdContainerById(string id, Claim currentUser)
        {
            // Todo : check if user is admin

            var adContainer = GetAdContainerModelById(id);
            if (!_organizationBusinessLogic.IsUserInOrganization(adContainer.Organization.Id.ToString(), currentUser.Value))
            {
                throw new ApiError(HttpStatusCode.Forbidden, "Cannot get the ad container of an organization you're not part of");
            }
            _repository.DeleteAdContainer(adContainer);
        }

        private List<TagModel> ResolveTags(List<string> tagNames)
        {
            return (from tagName in tagNames
                where !String.IsNullOrEmpty(tagName)
                select _tagBusinessLogic.GetTagModelByName(tagName)).ToList();
        }
    }
}