using System.Security.Claims;
using api.Helpers;
using api.Models.AdContainer;

namespace api.Business.AdContainer
{
    public interface IAdContainerBusinessLogic
    {
        AdContainerModel GetAdContainerById(string id);
        (int page, int pageSize, int maxPage, AdContainerModel[] tags) GetAdContainers(PagingDto paging, Claim currentUser);
        AdContainerModel AddNewAdContainer(AdContainerCreationModel newAdContainer, Claim currentUser);
        AdContainerModel UpdateAdContainerById(string id, AdContainerUpdateModel updatedAdContainer, Claim currentUser);
        void DeleteAdContainerById(string id, Claim currentUser);
    }
}