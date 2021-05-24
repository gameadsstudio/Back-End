using System.Collections.Generic;
using api.Helpers;
using api.Models.AdContainer;

namespace api.Business.AdContainer
{
    public interface IAdContainerBusinessLogic
    {
        AdContainerPublicDto GetAdContainerById(string id, ConnectedUser currentUser);
        (int page, int pageSize, int maxPage, List<AdContainerPublicDto> adContainers) GetAdContainers(PagingDto paging, string orgId, ConnectedUser currentUser);
        AdContainerPublicDto AddNewAdContainer(AdContainerCreationDto newAdContainer, ConnectedUser currentUser);
        AdContainerPublicDto UpdateAdContainerById(string id, AdContainerUpdateDto updatedAdContainer,
            ConnectedUser currentUser);
        void DeleteAdContainerById(string id, ConnectedUser currentUser);
    }
}