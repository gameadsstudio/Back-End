using System;
using api.Models.AdContainer;

namespace api.Repositories.AdContainer
{
    public interface IAdContainerRepository
    {
        AdContainerModel AddNewAdContainer(AdContainerModel adContainer);
        AdContainerModel GetAdContainerById(Guid id);
        AdContainerModel GetAdContainerByName(string name);
        int CountAdContainers();
        (AdContainerModel[], int) GetAdContainersByOrganizationId(int offset, int limit, Guid orgId);
        AdContainerModel UpdateAdContainer(AdContainerModel updatedAdContainer);
        int DeleteAdContainer(AdContainerModel adContainer);
    }
}