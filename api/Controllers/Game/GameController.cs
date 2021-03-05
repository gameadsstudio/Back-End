using api.Business.Game;
using api.Configuration;
using api.Contexts;
using api.Models.Game;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers.Game
{
    [Route("/v1/games")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameBusinessLogic _business;

        public GameController(ApiContext context, IOptions<AppSettings> appSettings)
        {
            _business = new GameBusinessLogic(context, appSettings);
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
        public IActionResult Patch(string id, [FromForm] GameUpdateModel newGame)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(_business.UpdateGameById(id, newGame));
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromForm] GameUpdateModel newGame)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(_business.UpdateGameById(id, newGame));
        }

        [AllowAnonymous]
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return Ok(_business.DeleteGameById(id));
        }
    }
}
