using api.Contexts;
using api.Models.Game;
using api.Errors;
using api.Repositories.Game;
using api.Business.Organization;
using AutoMapper;
using System.Net;
using api.Helpers;
using System.Collections.Generic;

namespace api.Business.Game
{
    public class GameBusinessLogic : IGameBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly IGameRepository _repository;
        private readonly IOrganizationBusinessLogic _organizationBusinessLogic;

        public GameBusinessLogic(ApiContext context, IMapper mapper,
            IOrganizationBusinessLogic organizationBusinessLogic)
        {
            _repository = new GameRepository(context);
            _organizationBusinessLogic = organizationBusinessLogic;
            _mapper = mapper;
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

            return _mapper.Map(_repository.AddNewGame(game), new GamePublicDto());
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

        public (int page, int pageSize, int totalItemCount, IList<GamePublicDto> games) GetGames(PagingDto paging)
        {
            paging = PagingHelper.Check(paging);
            var (games, totalItemCount) =
                _repository.GetGames((paging.Page - 1) * paging.PageSize, paging.PageSize);
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

            return _mapper.Map(_repository.UpdateGame(gameMapped), new GamePublicDto());
        }

        public void DeleteGameById(string id, ConnectedUser currentUser)
        {
            var game = GetGameModelById(id);

            if (!_organizationBusinessLogic.IsUserInOrganization(game.Organization.Id, currentUser.Id))
            {
                throw new ApiError(HttpStatusCode.Forbidden,
                    "Cannot update a game from an organization which you are not a part of");
            }

            _repository.DeleteGame(game);
        }
    }
}