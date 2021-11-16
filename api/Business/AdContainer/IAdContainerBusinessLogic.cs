using System.Collections.Generic;
using api.Helpers;
using api.Models.AdContainer;

namespace api.Business.AdContainer
{
    public interface IAdContainerBusinessLogic
    {
        AdContainerPublicDto GetAdContainerById(string id, ConnectedUser currentUser);
        (int page, int pageSize, int totalItemCount, List<AdContainerPublicDto> adContainers) GetAdContainers(PagingDto paging, AdContainerFiltersDto filters, ConnectedUser currentUser);
        AdContainerPublicDto AddNewAdContainer(AdContainerCreationDto newAdContainer, ConnectedUser currentUser);
        AdContainerPublicDto UpdateAdContainerById(string id, AdContainerUpdateDto updatedAdContainer,
            ConnectedUser currentUser);
        void DeleteAdContainerById(string id, ConnectedUser currentUser);
        public AdContainerModel GetAdContainerModelById(string id, ConnectedUser currentUser = null);
    }
}