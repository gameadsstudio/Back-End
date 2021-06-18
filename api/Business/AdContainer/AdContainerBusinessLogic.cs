using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using api.Business.Organization;
using api.Business.Tag;
using api.Business.Version;
using api.Contexts;
using api.Enums.User;
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
        private readonly IVersionBusinessLogic _versionBusinessLogic;

        public AdContainerBusinessLogic(
            ApiContext context,
            IMapper mapper,
            ITagBusinessLogic tagBusinessLogic,
            IOrganizationBusinessLogic organizationBusinessLogic,
            IVersionBusinessLogic versionBusinessLogic)
        {
            _repository = new AdContainerRepository(context);
            _tagBusinessLogic = tagBusinessLogic;
            _organizationBusinessLogic = organizationBusinessLogic;
            _versionBusinessLogic = versionBusinessLogic;
            _mapper = mapper;
        }

        public AdContainerPublicDto GetAdContainerById(string id, ConnectedUser currentUser)
        {
            var adContainer = GetAdContainerModelById(id);
            if (!_organizationBusinessLogic.IsUserInOrganization(adContainer.Organization.Id, currentUser.Id) &&
                currentUser.Role != UserRole.User)
            {
                throw new ApiError(HttpStatusCode.Forbidden,
                    "Cannot get the ad container of an organization you're not part of");
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
            string orgId, ConnectedUser currentUser)
        {
            paging = PagingHelper.Check(paging);
            var (adContainers, maxPage) = _repository.GetAdContainersByOrganizationId(
                (paging.Page - 1) * paging.PageSize,
                paging.PageSize,
                GuidHelper.StringToGuidConverter(orgId), currentUser.Id);
            return (paging.Page, paging.PageSize, (maxPage / paging.PageSize + 1),
                _mapper.Map(adContainers, new List<AdContainerPublicDto>()));
        }

        public AdContainerPublicDto AddNewAdContainer(AdContainerCreationDto newAdContainer, ConnectedUser currentUser)
        {
            var adContainer = _mapper.Map(newAdContainer, new AdContainerModel());
            adContainer.Version = _versionBusinessLogic.GetVersionModelById(newAdContainer.VersionId);

            Console.WriteLine("adContainer version id in creation: " + adContainer.Version.Id);
            Console.WriteLine("adContainer version game in creation: " + adContainer.Version.Game);
            Console.WriteLine("adContainer version name in creation: " + adContainer.Version.Name);
            Console.WriteLine("adContainer version semver in creation: " + adContainer.Version.SemVer);

            if (!_organizationBusinessLogic.IsUserInOrganization(
                GuidHelper.StringToGuidConverter(adContainer.Version.Game.Organization.Id.ToString()),
                currentUser.Id) && currentUser.Role != UserRole.User)
            {
                throw new ApiError(HttpStatusCode.Forbidden,
                    "Cannot create an ad container for an organization you're not part of");
            }

            adContainer.Organization = adContainer.Version.Game.Organization;
            adContainer.Tags = ResolveTags(newAdContainer.TagNames);
            return _mapper.Map(_repository.AddNewAdContainer(adContainer), new AdContainerPublicDto());
        }

        public AdContainerPublicDto UpdateAdContainerById(string id, AdContainerUpdateDto updatedAdContainer,
            ConnectedUser currentUser)
        {
            var adContainer = GetAdContainerModelById(id);

            if (!_organizationBusinessLogic.IsUserInOrganization(adContainer.Organization.Id, currentUser.Id) &&
                currentUser.Role == UserRole.User)
            {
                throw new ApiError(HttpStatusCode.Forbidden,
                    "Cannot get the ad container of an organization you're not part of");
            }

            _mapper.Map(updatedAdContainer, adContainer);

            if (updatedAdContainer.VersionId != null)
            {
                adContainer.Version =
                    _versionBusinessLogic.GetVersionModelById(
                        GuidHelper.StringToGuidConverter(updatedAdContainer.VersionId));
            }

            if (updatedAdContainer.TagNames != null)
            {
                adContainer.Tags = ResolveTags(updatedAdContainer.TagNames);
            }

            return _mapper.Map(_repository.UpdateAdContainer(adContainer), new AdContainerPublicDto());
        }

        public void DeleteAdContainerById(string id, ConnectedUser currentUser)
        {
            var adContainer = GetAdContainerModelById(id);
            if (!_organizationBusinessLogic.IsUserInOrganization(adContainer.Organization.Id, currentUser.Id) &&
                currentUser.Role == UserRole.User)
            {
                throw new ApiError(HttpStatusCode.Forbidden,
                    "Cannot get the ad container of an organization you're not part of");
            }

            _repository.DeleteAdContainer(adContainer);
        }

        private IList<TagModel> ResolveTags(IEnumerable<string> tagNames)
        {
            return (from tagName in tagNames
                where !string.IsNullOrEmpty(tagName)
                select _tagBusinessLogic.GetTagModelByName(tagName)).ToList();
        }
    }
}