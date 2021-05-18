using System;
using System.Collections.Generic;
using api.Models.Game;

namespace api.Repositories.Game
{
    public interface IGameRepository
    {
        GameModel AddNewGame(GameModel game);
        GameModel GetGameById(Guid id);
        GameModel UpdateGame(GameModel updatedGame);
        void DeleteGame(GameModel game);
        GameModel GetGameByName(string name);
        IList<GameModel> GetGames(int offset, int limit);
        int CountGames();
    }
}
