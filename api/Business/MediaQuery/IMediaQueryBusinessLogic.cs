using api.Enums.Media;
using api.Helpers;

namespace api.Business.MediaQuery
{
    public interface IMediaQueryBusinessLogic
    {
        object GetMedia(string adContainerId, Engine engine, ConnectedUser currentUser);
    }
}