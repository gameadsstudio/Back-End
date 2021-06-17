using System.Collections.Generic;
using api.Helpers;
using api.Models.Version;

namespace api.Business.Version
{
    public class VersionBusinessLogic : IVersionBusinessLogic
    {
        public VersionPublicDto GetVersionById(string id, ConnectedUser currentUser)
        {
            throw new System.NotImplementedException();
        }

        public (int page, int pageSize, int maxPage, IList<VersionPublicDto>) GetVersions(PagingDto paging, VersionFiltersDto filters,
            ConnectedUser currentUser)
        {
            throw new System.NotImplementedException();
        }

        public VersionPublicDto AddNewVersion(VersionCreationDto newVersion, ConnectedUser currentUser)
        {
            throw new System.NotImplementedException();
        }

        public VersionPublicDto UpdateVersionById(string id, VersionUpdateDto updatedVersion, ConnectedUser currentUser)
        {
            throw new System.NotImplementedException();
        }

        public int DeleteVersionById(string id, ConnectedUser currentUser)
        {
            throw new System.NotImplementedException();
        }
    }
}