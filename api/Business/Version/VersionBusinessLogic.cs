using System;
using System.Collections.Generic;
using System.Net;
using api.Business.Game;
using api.Business.Organization;
using api.Contexts;
using api.Errors;
using api.Helpers;
using api.Models.Version;
using api.Repositories.Version;
using AutoMapper;

namespace api.Business.Version
{
    public class VersionBusinessLogic : IVersionBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly IOrganizationBusinessLogic _organizationBusinessLogic;
        private readonly IGameBusinessLogic _gameBusinessLogic;
        private readonly IVersionRepository _repository;

        public VersionBusinessLogic(ApiContext context, IMapper mapper,
            IOrganizationBusinessLogic organizationBusinessLogic, IGameBusinessLogic gameBusinessLogic)
        {
            _repository = new VersionRepository(context);
            _organizationBusinessLogic = organizationBusinessLogic;
            _gameBusinessLogic = gameBusinessLogic;
            _mapper = mapper;
        }

        public VersionPublicDto GetVersionById(string id, ConnectedUser currentUser)
        {
            var version = GetVersionModelById(GuidHelper.StringToGuidConverter(id));

            if (!_organizationBusinessLogic.IsUserInOrganization(version.Game.Organization.Id, currentUser.Id))
            {
                throw new ApiError(HttpStatusCode.Forbidden,
                    "Cannot get a game version from an organization to which you don't belong.");
            }

            return _mapper.Map(version, new VersionPublicDto());
        }

        public (int page, int pageSize, int maxPage, IList<VersionPublicDto>) GetVersions(PagingDto paging,
            VersionFiltersDto filters, ConnectedUser currentUser)
        {
            paging = PagingHelper.Check(paging);
            var (versions, maxPage) =
                _repository.GetVersions((paging.Page - 1) * paging.PageSize, paging.PageSize, filters);
            return (paging.Page, paging.PageSize, maxPage, _mapper.Map(versions, new List<VersionPublicDto>()));
        }

        public VersionPublicDto AddNewVersion(VersionCreationDto newVersion, ConnectedUser currentUser)
        {
            var game = _gameBusinessLogic.GetGameModelById(newVersion.GameId.ToString());

            if (!_organizationBusinessLogic.IsUserInOrganization(game.Organization.Id, currentUser.Id))
            {
                throw new ApiError(HttpStatusCode.Forbidden,
                    "Cannot delete a game version from an organization to which you don't belong.");
            }

            if (VersionExistForGame(newVersion.SemVer, game.Id))
            {
                throw new ApiError(HttpStatusCode.Conflict,
                    $"Version with SemVer: {newVersion.SemVer} already exist for the game: {game.Id}");
            }

            var version = _mapper.Map(newVersion, new VersionModel());
            version.Game = game;
            var result = _repository.AddNewVersion(version);
            return _mapper.Map(result, new VersionPublicDto());
        }

        public VersionPublicDto UpdateVersionById(string id, VersionUpdateDto updatedVersion, ConnectedUser currentUser)
        {
            var version = this.GetVersionModelById(GuidHelper.StringToGuidConverter(id));

            if (!_organizationBusinessLogic.IsUserInOrganization(version.Game.Organization.Id, currentUser.Id))
            {
                throw new ApiError(HttpStatusCode.Forbidden,
                    "Cannot delete a game version from an organization to which you don't belong.");
            }

            if (updatedVersion.SemVer != null && VersionExistForGame(updatedVersion.SemVer, version.Game.Id))
            {
                throw new ApiError(HttpStatusCode.Conflict,
                    $"Version with SemVer: {updatedVersion.SemVer} already exist for the game: {version.Game.Id}");
            }

            version = _mapper.Map(updatedVersion, version);
            var result = _repository.UpdateVersion(version);
            return _mapper.Map(result, new VersionPublicDto());
        }

        public void DeleteVersionById(string id, ConnectedUser currentUser)
        {
            var version = this.GetVersionModelById(GuidHelper.StringToGuidConverter(id));

            if (!_organizationBusinessLogic.IsUserInOrganization(version.Game.Organization.Id, currentUser.Id))
            {
                throw new ApiError(HttpStatusCode.Forbidden,
                    "Cannot delete a game version from an organization to which you don't belong.");
            }

            _repository.DeleteVersion(version);
        }

        public VersionModel GetVersionModelById(Guid id)
        {
            var version = _repository.GetVersionById(id);

            if (version == null)
            {
                throw new ApiError(HttpStatusCode.NotFound, $"Couldn't find version with Id: {id}");
            }

            return version;
        }

        private bool VersionExistForGame(string semVer, Guid gameId)
        {
            var paging = new PagingDto();
            var filters = new VersionFiltersDto {SemVer = semVer, GameId = gameId};

            var (_, count) = _repository.GetVersions((paging.Page - 1) * paging.PageSize, paging.PageSize, filters);

            return count == 1;
        }
    }
}