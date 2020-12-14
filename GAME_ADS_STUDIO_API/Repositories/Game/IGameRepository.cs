using System.Collections.Generic;
using GAME_ADS_STUDIO_API.Models.Game;

namespace GAME_ADS_STUDIO_API.Repositories.Game
{
    public interface IGameRepository
    {
        int AddNewGame(GameModel Game);
        int UpdateGame(GameModel updatedGame, GameModel targetGame);
        int DeleteGame(GameModel Game);
    }
}
