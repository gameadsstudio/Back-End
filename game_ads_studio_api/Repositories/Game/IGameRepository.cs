using game_ads_studio_api.Models.Game;

namespace game_ads_studio_api.Repositories.Game
{
    public interface IGameRepository
    {
        int AddNewGame(GameModel game);
        int UpdateGame(GameModel updatedGame, GameModel targetGame);
        int DeleteGame(GameModel game);
    }
}
