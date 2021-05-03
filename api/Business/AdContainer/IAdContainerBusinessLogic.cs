using System.Security.Claims;
using api.Helpers;
using api.Models.AdContainer;

namespace api.Business.AdContainer
{
    public interface IAdContainerBusinessLogic
    {
        AdContainerPublicModel GetPublicAdContainerById(string id, Claim currentUser);
        (int page, int pageSize, int maxPage, AdContainerModel[] tags) GetAdContainers(PagingDto paging, string orgId, Claim currentUser);
        AdContainerPublicModel AddNewAdContainer(AdContainerCreationModel newAdContainer, Claim currentUser);
        AdContainerPublicModel UpdateAdContainerById(string id, AdContainerUpdateModel updatedAdContainer,
            Claim currentUser);
        void DeleteAdContainerById(string id, Claim currentUser);
    }
}