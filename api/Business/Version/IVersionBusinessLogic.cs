﻿using System;
using System.Collections.Generic;
using api.Helpers;
using api.Models.Version;

namespace api.Business.Version
{
    public interface IVersionBusinessLogic
    {
        VersionPublicDto GetVersionById(string id, ConnectedUser currentUser);

        (int page, int pageSize, int totalItemCount, IList<VersionPublicDto>) GetVersions(PagingDto paging,
            VersionFiltersDto filters, ConnectedUser currentUser);

        VersionPublicDto AddNewVersion(VersionCreationDto newVersion, ConnectedUser currentUser);
        VersionPublicDto UpdateVersionById(string id, VersionUpdateDto updatedVersion, ConnectedUser currentUser);
        void DeleteVersionById(string id, ConnectedUser currentUser);
        VersionModel GetVersionModelById(Guid id);
    }
}