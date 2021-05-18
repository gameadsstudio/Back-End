using System;
using System.Linq;
using api.Contexts;
using api.Models.Game;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories.Game
{
    public class GameRepository : IGameRepository
    {

        private readonly ApiContext _context;

        public GameRepository(ApiContext context)
        {
            _context = context;
        }

        public GameModel AddNewGame(GameModel game)
        {
            _context.Game.Add(game);
            _context.SaveChanges();
            return game;
        }

        public GameModel GetGameByName(string name)
        {
            return _context.Game.SingleOrDefault(a => a.Name == name);
        }

        public GameModel GetGameById(Guid id)
        {
            return _context.Game.Include(p => p.Organization).SingleOrDefault(e => e.Id == id);
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
