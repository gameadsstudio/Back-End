using api.Helpers;
using api.Models.Game;
using System.Collections.Generic;
using api.Models.Game;
using System.Security.Claims;

namespace api.Business.Game
{
    public interface IGameBusinessLogic
    {
        public GamePublicDto AddNewGame(GameCreationDto newGame, ConnectedUser currentUser);
        public GamePublicDto GetGameById(string id, ConnectedUser currentUser);
        GamePublicDto UpdateGameById(string id, GameUpdateDto updatedGame, ConnectedUser currentUser);
        void DeleteGameById(string id, ConnectedUser currentUser);
        (int page, int pageSize, int maxPage, IList<GamePublicDto> users) GetGames(PagingDto paging);
    }
}
