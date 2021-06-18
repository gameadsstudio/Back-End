using api.Helpers;
using api.Helpers;
using api.Models.Game;
using System.Collections.Generic;

namespace api.Business.Game
{
    public interface IGameBusinessLogic
    {
        public GamePublicDto AddNewGame(GameCreationDto newGame, ConnectedUser currentUser);
        public GamePublicDto GetGameById(string id, ConnectedUser currentUser);
        public GamePublicDto UpdateGameById(string id, GameUpdateDto updatedGame, ConnectedUser currentUser);
        public void DeleteGameById(string id, ConnectedUser currentUser);
        public (int page, int pageSize, int maxPage, IList<GamePublicDto> users) GetGames(PagingDto paging);
        public GameModel GetGameModelById(string id);
    }
}
