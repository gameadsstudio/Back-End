﻿using api.Models.Game;
using System.Security.Claims;

namespace api.Business.Game
{
    public interface IGameBusinessLogic
    {
        public GamePrivateDto AddNewGame(GameCreationDto newGame, Claim currentUser);
        public IGameDto GetGameById(string id, Claim currentUser);
        GameModel UpdateGameById(string id, GameUpdateDto updatedGame);
        int DeleteGameById(string id);
    }
}