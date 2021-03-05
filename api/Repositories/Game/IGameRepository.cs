using api.Models.Game;

namespace api.Repositories.Game
{
    public interface IGameRepository
    {
        int AddNewGame(GameModel game);
        int UpdateGame(GameModel updatedGame, GameModel targetGame);
        int DeleteGame(GameModel game);
    }
}
