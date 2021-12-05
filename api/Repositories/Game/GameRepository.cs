using System;
using System.Collections.Generic;
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
            return _context.Game.Include(a => a.Organization).SingleOrDefault(a => a.Id == id);
        }

        public void DeleteGame(GameModel game)
        {
            _context.Game.Remove(game);
            _context.SaveChanges();
        }

        public GameModel UpdateGame(GameModel updatedGame)
        {
            _context.Update(updatedGame);
            _context.SaveChanges();
            return updatedGame;
        }

        public (IList<GameModel>, int totalItemCount) GetGames(int offset, int limit, GameFiltersDto filters)
        {
            IQueryable<GameModel> query = _context.Game.OrderBy(p => p.DateCreation);
            
            if (filters.OrganizationId != Guid.Empty)
            {
                query = query.Where(game => game.Organization.Id == filters.OrganizationId);
            }
            
            return (query.Skip(offset).Take(limit).ToList(), query.Count());
        }

        public int CountGames()
        {
            return _context.Game.Count();
        }
    }
}