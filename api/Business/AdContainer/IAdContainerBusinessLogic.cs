using System.Collections.Generic;
using System.Security.Claims;
using api.Helpers;
using api.Models.AdContainer;

namespace api.Business.AdContainer
{
    public interface IAdContainerBusinessLogic
    {
        AdContainerPublicDto GetAdContainerById(string id, Claim currentUser);
        (int page, int pageSize, int maxPage, List<AdContainerPublicDto> adContainers) GetAdContainers(PagingDto paging, string orgId, Claim currentUser);
        AdContainerPublicDto AddNewAdContainer(AdContainerCreationDto newAdContainer, Claim currentUser);
        AdContainerPublicDto UpdateAdContainerById(string id, AdContainerUpdateDto updatedAdContainer,
            Claim currentUser);
        void DeleteAdContainerById(string id, Claim currentUser);
    }
}