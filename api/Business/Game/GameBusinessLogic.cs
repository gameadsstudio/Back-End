using System;
using api.Configuration;
using api.Contexts;
using api.Models.Game;
using api.Errors;
using api.Repositories.Game;
using System.Security.Claims;
using api.Business.Organization;
using api.Models.Organization;
using Microsoft.Extensions.Options;
using AutoMapper;
using System.Net;
using System.Linq;
using api.Helpers;

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

            var organization = _organizationBusinessLogic.GetOrganizationModelById(newGame.OrganizationId);

            if (organization.Users == null || organization.Users.All(user => user.Id.ToString() != currentUser.Value))
            {
                throw new ApiError(HttpStatusCode.Unauthorized,
                    "Cannot use an organization which you are not a part of");
            }

            game.Organization = organization;

            return _mapper.Map(_repository.AddNewGame(game), new GamePrivateDto());
        }

        public IGameDto GetGameById(string id, Claim currentUser)
        {
            var game = _repository.GetGameById(GuidHelper.StringToGuidConverter(id));

            //var organization = _organizationBusinessLogic.GetOrganizationModelById(game.Organization.Id.ToString());

            if (game.Organization.Users != null || game.Organization.Users.All(user => user.Id.ToString() == currentUser.Value))
            {
                return _mapper.Map(game, new GamePrivateDto());
            }

            return _mapper.Map(game, new GamePublicDto());
        }

        public GameModel UpdateGameById(string id, GameUpdateDto updatedGame)
        {
            throw new NotImplementedException();
        }

        public int DeleteGameById(string id)
        {
            throw new NotImplementedException();
        }
    }
}
