using api.Helpers;
using api.Models.Game;
using System.Collections.Generic;
using System.Security.Claims;

namespace api.Business.Game
{
    public interface IGameBusinessLogic
    {
        public GamePrivateDto AddNewGame(GameCreationDto newGame, Claim currentUser);
        public IGameDto GetGameById(string id, Claim currentUser);
        GamePrivateDto UpdateGameById(string id, GameUpdateDto updatedGame, Claim currentUser);
        void DeleteGameById(string id, Claim currentUser);
        (int page, int pageSize, int maxPage, List<GamePublicDto> users) GetGames(PagingDto paging);
    }
}