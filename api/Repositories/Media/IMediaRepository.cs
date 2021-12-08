using System;
using System.Collections.Generic;
using api.Enums.Media;
using api.Models.Media;
using api.Models.Media._2D;
using api.Models.Media._3D;
using api.Models.Media.Engine.Unity;
using api.Models.Tag;

namespace api.Repositories.Media
{
    public interface IMediaRepository
    {
        // Generic Medias
        MediaModel AddNewMedia(MediaModel mediaModel);
        MediaModel GetMediaById(Guid id);
        int CountMedias();
        (IList<MediaModel>, int) GetMediasByOrganizationId(int offset, int limit, Guid orgId, Guid userId);
        (IList<MediaModel>, int) GetMediasByTags(int offset, int limit, Guid orgId, Guid userId, IList<TagModel> tags);
        MediaModel UpdateMedia(MediaModel updatedMedia);
        int DeleteMedia(MediaModel mediaModel);

        // 2D Medias
        Media2DModel AddNew2DMedia(Media2DModel media);
        Media2DModel Get2DMediaById(Guid id);
        Media2DModel Get2DMediaByMediaId(Guid id);
        Media2DModel Update2DMedia(Media2DModel updatedMedia);
        int DeleteMedia(Media2DModel mediaModel);
        IList<Media2DModel> Get2DMediasByAspectRatio(AspectRatio aspectRatio);

        // 3D Medias
        Media3DModel AddNew3DMedia(Media3DModel media);
        Media3DModel Get3DMediaById(Guid id);
        Media3DModel Get3DMediaByMediaId(Guid id);
        Media3DModel Update3DMedia(Media3DModel updatedMedia);
        int DeleteMedia(Media3DModel mediaModel);
        IList<Media3DModel> Get3DMediasBySize(int width, int height, int depth);

        // Unity Medias
        MediaUnityModel AddNewUnityMedia(MediaUnityModel media);
        MediaUnityModel GetUnityMediaById(Guid id);
        MediaUnityModel GetUnityMediaByMediaId(Guid id);
        MediaUnityModel UpdateUnityMedia(MediaUnityModel updatedMedia);
        int DeleteMedia(MediaUnityModel mediaModel);

        // Media Query
        MediaUnityModel GetUnityMediaByFilters(MediaQueryFilters filters);
    }
}