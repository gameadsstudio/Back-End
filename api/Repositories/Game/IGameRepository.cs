using System;
using System.Collections.Generic;
using api.Models.Game;

namespace api.Repositories.Game
{
    public interface IGameRepository
    {
        GameModel AddNewGame(GameModel game);
        GameModel GetGameById(Guid id);
        int UpdateGame(GameModel updatedGame, GameModel targetGame);
        int DeleteGame(GameModel game);
        GameModel GetGameByName(string name);
        List<GameModel> GetGames(int offset, int limit);
        int CountGames();
    }
}
