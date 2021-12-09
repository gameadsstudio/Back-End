using System.Net;

namespace api.Errors
{
    public class GameNotFoundError : ApiError
    {
        public GameNotFoundError(string detail = "")
        {
            StatusCode = HttpStatusCode.NotFound;
            Error = "game-0001";
            ErrorMessage = "Game not found";
            Detail = detail;
        }
    }
    
    public class GameInsufficientRightsError : ApiError
    {
        public GameInsufficientRightsError(string detail = "")
        {
            StatusCode = HttpStatusCode.Forbidden;
            Error = "game-0002";
            ErrorMessage = "Insufficient rights for action";
            Detail = detail;
        }
    }
    
    public class GameNameAlreadyExistError : ApiError
    {
        public GameNameAlreadyExistError(string detail = "")
        {
            StatusCode = HttpStatusCode.Conflict;
            Error = "game-0003";
            ErrorMessage = "Game name already exist";
            Detail = detail;
        }
    }
}