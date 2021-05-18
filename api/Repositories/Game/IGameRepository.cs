using System;
using api.Models.Game;

namespace api.Repositories.Game
{
    public interface IGameRepository
    {
        GameModel AddNewGame(GameModel game);
        public GameModel GetGameById(Guid id);
        int UpdateGame(GameModel updatedGame, GameModel targetGame);
        int DeleteGame(GameModel game);
        public GameModel GetGameByName(string name);
    }
}
