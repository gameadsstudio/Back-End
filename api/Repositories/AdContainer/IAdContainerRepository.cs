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
        (List<AdContainerModel>, int totalItemCount) GetAdContainersByOrganizationId(int offset, int limit, Guid orgId, Guid userId);
        AdContainerModel UpdateAdContainer(AdContainerModel updatedAdContainer);
        int DeleteAdContainer(AdContainerModel adContainer);
    }
}