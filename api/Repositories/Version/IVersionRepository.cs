using System;
using System.Collections.Generic;
using api.Models.Version;

namespace api.Repositories.Version
{
    public interface IVersionRepository
    {
        public VersionModel AddNewVersion(VersionModel version);

        public VersionModel GetVersionById(Guid id);

        public (IList<VersionModel> versions, int totalItemCount) GetVersions(int offset, int limit, VersionFiltersDto filters);

        public VersionModel UpdateVersion(VersionModel updatedVersion);

        public int DeleteVersion(VersionModel version);
    }
}