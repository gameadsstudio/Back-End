using System.Collections.Generic;
using System.Net;
using api.Business.Organization;
using api.Business.Tag;
using api.Business.Version;
using api.Contexts;
using api.Enums.User;
using api.Errors;
using api.Helpers;
using api.Models.AdContainer;
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

        public AdContainerBusinessLogic(ApiContext context, IMapper mapper, ITagBusinessLogic tagBusinessLogic,
            IOrganizationBusinessLogic organizationBusinessLogic, IVersionBusinessLogic versionBusinessLogic)
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

        public AdContainerModel GetAdContainerModelById(string id, ConnectedUser currentUser = null)
        {
            var result = _repository.GetAdContainerById(GuidHelper.StringToGuidConverter(id));

            if (result == null)
            {
                throw new ApiError(HttpStatusCode.NotFound, $"Could not find ad container with Id: {id}");
            }

            if (currentUser == null) return result;

            if (!_organizationBusinessLogic.IsUserInOrganization(result.Organization.Id, currentUser.Id) &&
                currentUser.Role != UserRole.User)
            {
                throw new ApiError(HttpStatusCode.Forbidden,
                    "Cannot get the ad container of an organization you're not part of");
            }

            return result;
        }

        public (int page, int pageSize, int totalItemCount, List<AdContainerPublicDto> adContainers) GetAdContainers(
            PagingDto paging, AdContainerFiltersDto filters, ConnectedUser currentUser)
        {
            paging = PagingHelper.Check(paging);
            var (adContainers, totalItemCount) = _repository.GetAdContainers((paging.Page - 1) * paging.PageSize,
                paging.PageSize, filters, currentUser.Id);
            return (paging.Page, paging.PageSize, totalItemCount,
                _mapper.Map(adContainers, new List<AdContainerPublicDto>()));
        }

        public AdContainerPublicDto AddNewAdContainer(AdContainerCreationDto newAdContainer, ConnectedUser currentUser)
        {
            var adContainer = _mapper.Map(newAdContainer, new AdContainerModel());
            adContainer.Version = _versionBusinessLogic.GetVersionModelById(newAdContainer.VersionId);

            if (!_organizationBusinessLogic.IsUserInOrganization(
                GuidHelper.StringToGuidConverter(adContainer.Version.Game.Organization.Id.ToString()),
                currentUser.Id) && currentUser.Role != UserRole.User)
            {
                throw new ApiError(HttpStatusCode.Forbidden,
                    "Cannot create an ad container for an organization you're not part of");
            }

            adContainer.Organization = adContainer.Version.Game.Organization;
            adContainer.Tags = _tagBusinessLogic.ResolveTags(newAdContainer.TagNames);
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
                adContainer.Tags = _tagBusinessLogic.ResolveTags(updatedAdContainer.TagNames);
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
    }
}