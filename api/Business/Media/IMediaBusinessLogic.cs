using System;
using System.Collections.Generic;
using api.Enums.Media;
using api.Helpers;
using api.Models.Media;
using api.Models.Media.Engine.Unity;

namespace api.Business.Media
{
    public interface IMediaBusinessLogic
    {
        MediaPublicDto GetMediaById(string id, ConnectedUser currentUser);
        (int page, int pageSize, int maxPage, IList<MediaPublicDto> medias) GetMedias(PagingDto paging, IList<string> tagNames, string orgId, ConnectedUser currentUser);
        MediaPublicDto AddNewMedia(MediaCreationDto newMedia, ConnectedUser currentUser);
        MediaPublicDto UpdateMediaById(string id, MediaUpdateDto updatedMedia,
            ConnectedUser currentUser);
        MediaPublicDto GetEngineMediaById(string id, ConnectedUser currentUser, Engine engine);
        void DeleteMediaById(string id, ConnectedUser currentUser);
        MediaUnityPublicDto AddNewMediaUnity(MediaUnityCreationDto newMediaUnity, string mediaId);
        MediaUnityPublicDto UpdateMediaUnityState(MediaState newState, string id, string mediaId);
        MediaPublicDto UpdateMediaState(MediaState newState, string mediaId);
        MediaModel GetMediaModelById(string mediaId);
    }
}