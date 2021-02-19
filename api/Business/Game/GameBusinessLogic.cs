using System;
using api.Configuration;
using api.Contexts;
using api.Models.Game;
using api.Repositories.Game;
using Microsoft.Extensions.Options;

namespace api.Business.Game
{
    public class GameBusinessLogic : IGameBusinessLogic
    {
        private readonly IGameRepository _repository;
        private readonly AppSettings _appSettings;

        public GameBusinessLogic(ApiContext context, IOptions<AppSettings> appSettings)
        {
            _repository = new GameRepository(context);
            _appSettings = appSettings.Value;
        }

        public GameModel AddNewGame(GameCreationModel newGame)
        {
            var game = new GameModel
            {
                OrganizationId = Guid.Parse(newGame.OrganizationId),
                MediaId = Guid.Parse(newGame.MediaId),
                Name = newGame.Name,
                Status = newGame.Status,
                DateCreation = DateTime.Now,
                DateLaunch = DateTime.Now,
                DateUpdate = DateTime.Now
            };

            throw new NotImplementedException();
        }

        public GameModel UpdateGameById(string id, GameUpdateModel updatedGame)
        {
            throw new NotImplementedException();
        }

        public int DeleteGameById(string id)
        {
            throw new NotImplementedException();
        }
    }
}
