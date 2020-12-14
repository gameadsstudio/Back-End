using GAME_ADS_STUDIO_API.Models.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAME_ADS_STUDIO_API.Business.Game
{
    interface IGameBusinessLogic
    {
        GameModel AddNewGame(GameCreationModel newGame);
        int UpdateGameById(int id, GameUpdateModel updatedGame);
        int DeleteGameById(int id);
    }
}