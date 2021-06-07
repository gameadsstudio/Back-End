using System;
using api.Contexts;
using api.Models.Game;
using api.Errors;
using api.Repositories.Game;
using System.Security.Claims;
using api.Business.Organization;
using AutoMapper;
using System.Net;
using System.Linq;
using api.Helpers;
using System.Collections.Generic;

namespace api.Business.Game
{
    public class GameBusinessLogic : IGameBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly IGameRepository _repository;
        private readonly IOrganizationBusinessLogic _organizationBusinessLogic;

        public GameBusinessLogic(ApiContext context, IMapper mapper, IOrganizationBusinessLogic organizationBusinessLogic)
        {
            _repository = new GameRepository(context);
            _organizationBusinessLogic = organizationBusinessLogic;
            _mapper = mapper;
        }

        public GamePrivateDto AddNewGame(GameCreationDto newGame, Claim currentUser)
        {
            var game = _mapper.Map(newGame, new GameModel());

            if (_repository.GetGameByName(game.Name) != null)
            {
                throw new ApiError(HttpStatusCode.Conflict,
                    $"Game with name: {game.Name} already exists");
            }

            if (!_organizationBusinessLogic.IsUserInOrganization(newGame.OrgId, currentUser.Value))
            {
                throw new ApiError(HttpStatusCode.Forbidden, "Cannot create a game for an organization you're not part of");
            }

            game.Organization = _organizationBusinessLogic.GetOrganizationModelById(newGame.OrgId);

            return _mapper.Map(_repository.AddNewGame(game), new GamePrivateDto());
        }

        public GamePublicDto GetGameById(string id, Claim currentUser)
        {
            var game = _repository.GetGameById(GuidHelper.StringToGuidConverter(id));

            if (game == null)
            {
                throw new ApiError(HttpStatusCode.NotFound, $"Couldn't find game with Id: {id}");
            }

            if (!_organizationBusinessLogic.IsUserInOrganization(game.Organization.Id.ToString(), currentUser.Value))
            {
                throw new ApiError(HttpStatusCode.Forbidden,
                "Cannot fetch a game from an organization which you are not a part of");
            }
            return _mapper.Map(game, new GamePublicDto());
        }

        public (int, int, int, IList<GamePublicDto>) GetGames(PagingDto paging)
        {
            paging = PagingHelper.Check(paging);
            var maxPage = _repository.CountGames() / paging.PageSize + 1;
            var games = _repository.GetGames((paging.Page - 1) * paging.PageSize, paging.PageSize);
            return (paging.Page, paging.PageSize, maxPage, _mapper.Map(games, new List<GamePublicDto>()));
        }

        public GamePrivateDto UpdateGameById(string id, GameUpdateDto updatedGame, Claim currentUser)
        {
            var game = _repository.GetGameById(GuidHelper.StringToGuidConverter(id));

            if (game == null)
            {
                throw new ApiError(HttpStatusCode.NotFound, $"Couldn't find game with Id: {id}");
            }

            if (game.Organization.Users != null && game.Organization.Users.All(user => user.Id.ToString() == currentUser.Value))
            {
                var gameMapped = _mapper.Map(updatedGame, game);

                return _mapper.Map(_repository.UpdateGame(gameMapped), new GamePrivateDto());
            }
            throw new ApiError(HttpStatusCode.Forbidden,
                "Cannot remove a game from an organization which you are not a part of");
        }

        public void DeleteGameById(string id, Claim currentUser)
        {
            var game = _repository.GetGameById(GuidHelper.StringToGuidConverter(id));

            if (game == null)
            {
                throw new ApiError(HttpStatusCode.NotFound, $"Couldn't find game with Id: {id}");
            }

            if (game.Organization.Users != null && game.Organization.Users.All(user => user.Id.ToString() == currentUser.Value))
            {
                _repository.DeleteGame(game);
            }
            else
            {
                throw new ApiError(HttpStatusCode.Forbidden,
                    "Cannot delete a game which you are not a part of the organization game owner");
            }
        }
    }
}
