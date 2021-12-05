using api.Business.Game;
using api.Models.Game;
using Microsoft.AspNetCore.Mvc;
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
        public ActionResult<GetDto<GamePublicDto>> Post([FromForm] GameCreationDto newGame)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Created("Game", new GetDto<GamePublicDto>(_business.AddNewGame(newGame, currentUser)));
        }

        [HttpGet("{id}")]
        public ActionResult<GetDto<GamePublicDto>> GetGame(string id)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Ok(new GetDto<object>(_business.GetGameById(id, currentUser)));
        }

        [HttpGet]
        public ActionResult<GetAllDto<GamePublicDto>> GetAll([FromQuery] PagingDto paging, [FromQuery] GameFiltersDto filters)
        {
            var currentUser = new ConnectedUser(User.Claims);
            
            return Ok(new GetAllDto<GamePublicDto>(_business.GetGames(paging, filters, currentUser)));
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(string id, [FromForm] GameUpdateDto newGame)
        {
            var currentUser = new ConnectedUser(User.Claims);

            return Ok(new GetDto<object>(_business.UpdateGameById(id, newGame, currentUser)));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var currentUser = new ConnectedUser(User.Claims);

            _business.DeleteGameById(id, currentUser);

            return Ok();
        }
    }
}