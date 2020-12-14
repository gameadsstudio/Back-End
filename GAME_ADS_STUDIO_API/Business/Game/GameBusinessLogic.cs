using GAME_ADS_STUDIO_API.Repositories.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GAME_ADS_STUDIO_API.Configuration;
using GAME_ADS_STUDIO_API.Contexts;
using Microsoft.Extensions.Options;
using GAME_ADS_STUDIO_API.Models.Game;

namespace GAME_ADS_STUDIO_API.Business.Game
{
    public class GameBusinessLogic : IGameBusinessLogic
    {
        private readonly IGameRepository _repository;
        private readonly AppSettings _appSettings;

        public GameBusinessLogic(IOptions<AppSettings> appSettings)
        {
            //_repository = new GameRepository(context);
            _appSettings = appSettings.Value;
        }

        public GameModel AddNewGame(GameCreationModel newGame)
        {
            var game = new GameModel();

            game.Org_id = newGame.Org_id;
            game.Media_id = newGame.Media_id;
            game.Game_name = newGame.Game_name;
            game.Game_status = newGame.Game_status;
            game.Game_date_creation = DateTime.Now;
            game.Game_date_launch = DateTime.Now;
            game.Game_date_update = DateTime.Now;

            return game;
        }

        public int DeleteGameById(int id)
        {
            return 1;
        }

        public int UpdateGameById(int id, GameUpdateModel updatedGame)
        {
            return 1;
        }
    }
}
