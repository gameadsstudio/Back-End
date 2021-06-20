using System.Collections.Generic;
using api.Helpers;
using api.Models.Media;

namespace api.Business.Media
{
    public interface IMediaBusinessLogic
    {
        MediaPublicDto GetMediaById(string id, ConnectedUser currentUser);
        (int page, int pageSize, int maxPage, IList<MediaPublicDto> medias) GetMedias(PagingDto paging, string versionId, ConnectedUser currentUser);
        MediaPublicDto AddNewMedia(MediaCreationDto newMedia, ConnectedUser currentUser);
        MediaPublicDto UpdateMediaById(string id, MediaUpdateDto updatedMedia,
            ConnectedUser currentUser);
        // Todo : Check how to change engine string to enum
        MediaPublicDto GetEngineMediaById(string id, ConnectedUser currentUser, string engine);
        void DeleteMediaById(string id, ConnectedUser currentUser);
    }
}