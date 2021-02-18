using System;
using api.Contexts;
using api.Models.Game;

namespace api.Repositories.Game
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
