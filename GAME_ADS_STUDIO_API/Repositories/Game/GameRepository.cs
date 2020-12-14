using System;
using System.Collections.Generic;
using GAME_ADS_STUDIO_API.Contexts;
using GAME_ADS_STUDIO_API.Models.Game;

namespace GAME_ADS_STUDIO_API.Repositories.Game
{
    public class GameRepository : IGameRepository
    {

        private readonly ApiContext _context;

        public GameRepository(ApiContext context)
        {
            _context = context;
        }

        public int AddNewGame(GameModel Game)
        {
            throw new NotImplementedException();
        }

        public int DeleteGame(GameModel Game)
        {
            throw new NotImplementedException();
        }

        public int UpdateGame(GameModel updatedGame, GameModel targetGame)
        {
            throw new NotImplementedException();
        }

    }
}
