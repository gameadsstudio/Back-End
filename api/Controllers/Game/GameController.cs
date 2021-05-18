﻿using api.Business.Game;
using api.Configuration;
using api.Contexts;
using api.Models.Game;
using System.Security.Claims;
using api.Models.Organization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using api.Helpers;
using api.Models.Common;

namespace api.Controllers.Game
{
    [Route("/v1/games")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameBusinessLogic _business;

        public GameController(IGameBusinessLogic gameBusinessLogic)
        {
            _business = gameBusinessLogic;
        }

        [HttpPost]
        public ActionResult<GetDto<GamePrivateDto>> Post([FromForm] GameCreationDto newGame)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);

            return Created("Game", new GetDto<GamePrivateDto>(_business.AddNewGame(newGame, currentUser)));
        }

        [HttpGet("{id}")]
        public ActionResult<GetDto<IGameDto>> GetGame(string id)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);

            return Ok(new GetDto<IGameDto>(_business.GetGameById(id, currentUser)));
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(string id, [FromForm] GameUpdateDto newGame)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(_business.UpdateGameById(id, newGame));
        }

        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromForm] GameUpdateDto newGame)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(_business.UpdateGameById(id, newGame));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return Ok(_business.DeleteGameById(id));
        }
    }
}
