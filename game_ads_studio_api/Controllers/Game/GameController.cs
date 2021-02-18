using game_ads_studio_api.Business.Game;
using game_ads_studio_api.Configuration;
using game_ads_studio_api.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using game_ads_studio_api.Models.Game;
using Microsoft.AspNetCore.Authorization;

namespace game_ads_studio_api.Controllers.Game
{
    [Route("/api/game")]
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
