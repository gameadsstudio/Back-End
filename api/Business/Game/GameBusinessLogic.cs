using api.Contexts;
using api.Models.Game;
using api.Errors;
using api.Repositories.Game;
using api.Business.Organization;
using AutoMapper;
using System.Net;
using api.Helpers;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace api.Business.Game
{
    public class GameBusinessLogic : IGameBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly IGameRepository _repository;
        private readonly IOrganizationBusinessLogic _organizationBusinessLogic;
        private readonly UriHelper _uriHelper;

        public GameBusinessLogic(ApiContext context, IMapper mapper,
            IOrganizationBusinessLogic organizationBusinessLogic, IHttpContextAccessor httpContextAccessor)
        {
            _repository = new GameRepository(context);
            _organizationBusinessLogic = organizationBusinessLogic;
            _mapper = mapper;
            _uriHelper = new UriHelper(httpContextAccessor);
        }

        public GamePublicDto AddNewGame(GameCreationDto newGame, ConnectedUser currentUser)
        {
            var game = _mapper.Map(newGame, new GameModel());

            if (_repository.GetGameByName(game.Name) != null)
            {
                throw new ApiError(HttpStatusCode.Conflict, $"Game with name: {game.Name} already exists");
            }

            if (!_organizationBusinessLogic.IsUserInOrganization(newGame.OrgId, currentUser.Id))
            {
                throw new ApiError(HttpStatusCode.Forbidden,
                    "Cannot create a game for an organization you're not part of");
            }

            game.Organization = _organizationBusinessLogic.GetOrganizationModelById(newGame.OrgId);

            var result = _repository.AddNewGame(game);
            
            result = AddOrReplaceGameMiniature(result, newGame.Miniature);
            
            return _mapper.Map(result, new GamePublicDto());
        }

        private GameModel AddOrReplaceGameMiniature(GameModel game, IFormFile miniature)
        {
            if (miniature == null)
            {
                return game;
            }

            var assetsDir = $"/assets/games/{game.Id.ToString()}";

            // Create user dir if not exists
            if (!Directory.Exists(assetsDir))
            {
                Directory.CreateDirectory(assetsDir);
            }

            // TODO check for file format before saving
            // Saving texture
            using (var fileStream = new FileStream($"{assetsDir}/miniature{Path.GetExtension(miniature.FileName)}",
                FileMode.Create))
            {
                miniature.CopyTo(fileStream);
                game.MiniatureUrl = _uriHelper.UriBuilder(fileStream.Name);
            }

            return _repository.UpdateGame(game);
        }

        public GamePublicDto GetGameById(string id, ConnectedUser currentUser)
        {
            var game = _repository.GetGameById(GuidHelper.StringToGuidConverter(id));

            if (game == null)
            {
                throw new ApiError(HttpStatusCode.NotFound, $"Couldn't find game with Id: {id}");
            }

            if (!_organizationBusinessLogic.IsUserInOrganization(game.Organization.Id, currentUser.Id))
            {
                throw new ApiError(HttpStatusCode.Forbidden,
                    "Cannot fetch a game from an organization which you are not a part of");
            }

            return _mapper.Map(game, new GamePublicDto());
        }

        public GameModel GetGameModelById(string id)
        {
            return _repository.GetGameById(GuidHelper.StringToGuidConverter(id)) ??
                   throw new ApiError(HttpStatusCode.NotFound, $"Could not find a game with Id: {id}");
        }

        public (int page, int pageSize, int totalItemCount, IList<GamePublicDto> games) GetGames(PagingDto paging, GameFiltersDto filters, ConnectedUser user)
        {
            paging = PagingHelper.Check(paging);

            if (!_organizationBusinessLogic.IsUserInOrganization(filters.OrganizationId, user.Id))
            {
                throw new ApiError(HttpStatusCode.Forbidden,
                    "Cannot get games from organization which you are not a part of");
            }
            
            var (games, totalItemCount) = _repository.GetGames((paging.Page - 1) * paging.PageSize, paging.PageSize, filters);
            
            return (paging.Page, paging.PageSize, totalItemCount, _mapper.Map(games, new List<GamePublicDto>()));
        }

        public GamePublicDto UpdateGameById(string id, GameUpdateDto updatedGame, ConnectedUser currentUser)
        {
            var game = GetGameModelById(id);

            if (!_organizationBusinessLogic.IsUserInOrganization(game.Organization.Id, currentUser.Id))
            {
                throw new ApiError(HttpStatusCode.Forbidden,
                    "Cannot update a game from an organization which you are not a part of");
            }

            var gameMapped = _mapper.Map(updatedGame, game);

            var result = _repository.UpdateGame(gameMapped);
            
            result = AddOrReplaceGameMiniature(result, updatedGame.Miniature);
            
            return _mapper.Map(result, new GamePublicDto());
        }

        public void DeleteGameById(string id, ConnectedUser currentUser)
        {
            var game = GetGameModelById(id);

            if (!_organizationBusinessLogic.IsUserInOrganization(game.Organization.Id, currentUser.Id))
            {
                throw new ApiError(HttpStatusCode.Forbidden,
                    "Cannot update a game from an organization which you are not a part of");
            }
            
            var assetsDir = $"/assets/games/{game.Id.ToString()}";
            
            // Delete assets dir if exists
            if (Directory.Exists(assetsDir))
            {
                Directory.Delete(assetsDir, true);
            }

            _repository.DeleteGame(game);
        }
    }
}