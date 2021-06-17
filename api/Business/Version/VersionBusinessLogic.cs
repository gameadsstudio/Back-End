using System.Collections.Generic;
using api.Contexts;
using api.Helpers;
using api.Models.Version;
using api.Repositories.Version;
using AutoMapper;

namespace api.Business.Version
{
    public class VersionBusinessLogic : IVersionBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly IVersionRepository _repository;
        
        public VersionBusinessLogic(ApiContext context, IMapper mapper)
        {
            _repository = new VersionRepository(context);
            _mapper = mapper;
        }
        
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