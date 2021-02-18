using api.Models.Game;

namespace api.Business.Game
{
    internal interface IGameBusinessLogic
    {
        GameModel AddNewGame(GameCreationModel newGame);
        GameModel UpdateGameById(string id, GameUpdateModel updatedGame);
        int DeleteGameById(string id);
    }
}