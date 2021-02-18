using game_ads_studio_api.Models.Game;

namespace game_ads_studio_api.Business.Game
{
    internal interface IGameBusinessLogic
    {
        GameModel AddNewGame(GameCreationModel newGame);
        GameModel UpdateGameById(string id, GameUpdateModel updatedGame);
        int DeleteGameById(string id);
    }
}