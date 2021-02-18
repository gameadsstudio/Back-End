using System;
using game_ads_studio_api.Contexts;
using game_ads_studio_api.Models.Game;

namespace game_ads_studio_api.Repositories.Game
{
    public class GameRepository : IGameRepository
    {

        private readonly ApiContext _context;

        public GameRepository(ApiContext context)
        {
            _context = context;
        }

        public int AddNewGame(GameModel game)
        {
            throw new NotImplementedException();
        }

        public int DeleteGame(GameModel game)
        {
            throw new NotImplementedException();
        }

        public int UpdateGame(GameModel updatedGame, GameModel targetGame)
        {
            throw new NotImplementedException();
        }

    }
}
