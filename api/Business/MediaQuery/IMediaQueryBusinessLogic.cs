using System.Collections.Generic;
using api.Helpers;
using api.Models.Media;

namespace api.Business.MediaQuery
{
    public interface IMediaQueryBusinessLogic
    {
        MediaPublicDto GetMedia(PagingDto paging, IList<string> tagNames, string adContainerId, ConnectedUser currentUser);
    }
}