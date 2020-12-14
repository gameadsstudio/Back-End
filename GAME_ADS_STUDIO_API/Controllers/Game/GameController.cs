using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GAME_ADS_STUDIO_API.Business.Game;
using GAME_ADS_STUDIO_API.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using GAME_ADS_STUDIO_API.Configuration;
using Microsoft.AspNetCore.Authorization;
using GAME_ADS_STUDIO_API.Models.Game;
namespace GAME_ADS_STUDIO_API.Controllers.Game
{
    [Route("/api/game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameBusinessLogic _business;

        public GameController(IOptions<AppSettings> appSettings)
        {
            _business = new GameBusinessLogic(appSettings);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Post([FromForm] GameCreationModel newGame)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var success = _business.AddNewGame(newGame);

            if (success != null)
                return Created("Game", success);
            return Conflict(new { message = "Couldn't create Game" });
        }

        [AllowAnonymous]
        [HttpPatch("{id}")]
        public IActionResult Patch(int id, [FromForm] GameUpdateModel newGame)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var success = _business.UpdateGameById(id, newGame);

            return success switch
            {
                1 => (IActionResult)Ok(),
                2 => NotFound(),
                _ => BadRequest()
            };
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromForm] GameUpdateModel newGame)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var success = _business.UpdateGameById(id, newGame);

            return success switch
            {
                1 => (IActionResult)Ok(),
                2 => NotFound(),
                _ => BadRequest()
            };
        }

        [AllowAnonymous]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var success = _business.DeleteGameById(id);

            return success switch
            {
                1 => (IActionResult)Ok(),
                2 => Unauthorized(),
                _ => BadRequest()
            };
        }

    }
}
