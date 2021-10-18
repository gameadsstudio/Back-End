using api.Enums.Media;
using api.Helpers;
using api.Models.Media;

namespace api.Business.MediaQuery
{
    public interface IMediaQueryBusinessLogic
    {
        MediaPublicDto GetMedia(string adContainerId, Engine engine, ConnectedUser currentUser);
    }
}