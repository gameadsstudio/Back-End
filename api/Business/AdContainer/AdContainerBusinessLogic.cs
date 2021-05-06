using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
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
        // private readonly IOrganizationBusinessLogic _organizationBusinessLogic;

        public AdContainerBusinessLogic(
                ApiContext context,
                IMapper mapper,
                ITagBusinessLogic tagBusinessLogic)
            // IOrganizationBusinessLogic organizationBusinessLogic)
        {
            _repository = new AdContainerRepository(context);
            _tagBusinessLogic = tagBusinessLogic;
            // _organizationBusinessLogic = organizationBusinessLogic;
            _mapper = mapper;
        }

        public AdContainerPublicDto GetPublicAdContainerById(string id, Claim currentUser)
        {
            // Todo : check if user is in the specified org OR the user is admin
            return _mapper.Map(GetAdContainerById(id, currentUser), new AdContainerPublicDto());
        }

        private AdContainerModel GetAdContainerById(string id, Claim currentUser)
        {
            // Todo : check if current user has access to the ad container
            return _repository.GetAdContainerById(GuidHelper.StringToGuidConverter(id)) ??
                   throw new ApiError(HttpStatusCode.NotFound, $"Could not find ad container with Id: {id}");
        }

        public (int page, int pageSize, int maxPage, AdContainerModel[] tags) GetAdContainers(PagingDto paging,
            string orgId, Claim currentUser)
        {
            // Todo : check if user is in the specified org OR the user is admin

            paging = PagingHelper.Check(paging);
            var (adContainers, maxPage) = _repository.GetAdContainersByOrganizationId(
                (paging.Page - 1) * paging.PageSize,
                paging.PageSize,
                GuidHelper.StringToGuidConverter(orgId));
            return (paging.Page, paging.PageSize, (maxPage / paging.PageSize + 1), adContainers);
        }

        public AdContainerPublicDto AddNewAdContainer(AdContainerCreationDto newAdContainer, Claim currentUser)
        {
            // Todo : check if user is in the specified org OR the user is admin

            var adContainer = _mapper.Map(newAdContainer, new AdContainerModel());
            adContainer.Tags = ResolveTags(newAdContainer.TagNames);
            /*
             * Todo : Add Organization and version to model
             */
            // adContainer.Organization = _organizationBusinessLogic.GetOrganizationById(Guid.Parse(newAdContainer.OrgId));
            return _mapper.Map(_repository.AddNewAdContainer(adContainer), new AdContainerPublicDto());
        }

        public AdContainerPublicDto UpdateAdContainerById(string id, AdContainerUpdateDto updatedAdContainer,
            Claim currentUser)
        {
            // Todo : check if user is in the specified org OR the user is admin

            var adContainer = _mapper.Map(updatedAdContainer, GetAdContainerById(id, currentUser));
            if (updatedAdContainer.TagNames.Count > 0)
            {
                adContainer.Tags = ResolveTags(updatedAdContainer.TagNames);
            }

            return _mapper.Map(_repository.UpdateAdContainer(adContainer), new AdContainerPublicDto());
        }

        public void DeleteAdContainerById(string id, Claim currentUser)
        {
            // Todo : check if user is in the specified org OR the user is admin

            var adContainer = GetAdContainerById(id, currentUser);
            _repository.DeleteAdContainer(adContainer);
        }

        private List<TagModel> ResolveTags(List<string> tagNames)
        {
            return tagNames.Select(tagName => _tagBusinessLogic.GetTagModelByName(tagName)).ToList();
        }
    }
}