using System;
using System.Collections.Generic;
using api.Enums.Media;
using api.Helpers;
using api.Models.Media;
using api.Models.Media._2D;
using api.Models.Media._3D;
using api.Models.Media.Engine.Unity;

namespace api.Business.Media
{
    public interface IMediaBusinessLogic
    {
        MediaPublicDto GetMediaById(string id, ConnectedUser currentUser);
        (int page, int pageSize, int totalItemCount, IList<MediaPublicDto> medias) GetMedias(PagingDto paging, IList<string> tagNames, string orgId, ConnectedUser currentUser);
        MediaPublicDto AddNewMedia(MediaCreationDto newMedia, ConnectedUser currentUser);
        MediaPublicDto UpdateMediaById(string id, MediaUpdateDto updatedMedia,
            ConnectedUser currentUser);
        MediaPublicDto GetEngineMediaById(string id, ConnectedUser currentUser, Engine engine);
        MediaUnityModel GetEngineMedia(MediaQueryFilters filter);
        void DeleteMediaById(string id, ConnectedUser currentUser);
        MediaUnityPublicDto AddNewMediaUnity(MediaUnityCreationDto newMediaUnity, string mediaId);
        MediaPublicDto UpdateMediaState(MediaState newState, string mediaId);
        MediaModel GetMediaModelById(string mediaId);
        MediaPublicDto RetryBuild(string id, ConnectedUser currentUser);
        MediaUnityPublicDto UpdateMediaUnity(MediaUnityUpdateDto updatedUnityMedia, string mediaId);
        IEnumerable<Guid> GetMedia2DIds(AspectRatio aspectRatio);
        IEnumerable<Guid> GetMedia3DIds(int width, int height, int depth);

        public MediaUnityPublicDto GetMediaUnityPublicDtoByMediaId(string mediaId);
    }
}