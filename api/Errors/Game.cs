using System.Net;

namespace api.Errors
{
    public class GameNotFoundError : ApiError
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        public string ErrorMessage { get; set; }
        public string Detail { get; set; }

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
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        public string ErrorMessage { get; set; }
        public string Detail { get; set; }

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
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        public string ErrorMessage { get; set; }
        public string Detail { get; set; }

        public GameNameAlreadyExistError(string detail = "")
        {
            StatusCode = HttpStatusCode.Conflict;
            Error = "game-0003";
            ErrorMessage = "Game name already exist";
            Detail = detail;
        }
    }
}