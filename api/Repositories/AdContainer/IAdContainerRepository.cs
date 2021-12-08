using System;
using System.Collections.Generic;
using api.Models.AdContainer;

namespace api.Repositories.AdContainer
{
    public interface IAdContainerRepository
    {
        AdContainerModel AddNewAdContainer(AdContainerModel adContainer);
        AdContainerModel GetAdContainerById(Guid id);
        AdContainerModel GetAdContainerByName(string name);
        (List<AdContainerModel>, int totalItemCount) GetAdContainers(int offset, int limit, AdContainerFiltersDto filters, Guid userId);
        AdContainerModel UpdateAdContainer(AdContainerModel updatedAdContainer);
        int DeleteAdContainer(AdContainerModel adContainer);
        int DeleteAdContainersForVersion(Guid versionId);
    }
}