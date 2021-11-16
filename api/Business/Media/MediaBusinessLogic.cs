using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using api.Business.Organization;
using api.Business.Tag;
using api.Contexts;
using api.Enums.Media;
using api.Errors;
using api.Helpers;
using api.Models.Media;
using api.Models.Media._2D;
using api.Models.Media._3D;
using api.Models.Media.Engine.Unity;
using api.Models.Tag;
using api.Repositories.Media;
using api.Services.RabbitMQ;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Type = api.Enums.Media.Type;

namespace api.Business.Media
{
    public class MediaBusinessLogic : IMediaBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly IMediaRepository _repository;
        private readonly IOrganizationBusinessLogic _organizationBusiness;
        private readonly ITagBusinessLogic _tagBusiness;
        private readonly IClient _unityRmqClient;
        private readonly UriHelper _uriHelper;

        public MediaBusinessLogic(ApiContext context, IMapper mapper, IOrganizationBusinessLogic organizationBusiness,
            ITagBusinessLogic tagBusiness, IHttpContextAccessor httpContextAccessor)
        {
            _repository = new MediaRepository(context);
            _mapper = mapper;
            _organizationBusiness = organizationBusiness;
            _tagBusiness = tagBusiness;
            _unityRmqClient = new Client("unity");
            _uriHelper = new UriHelper(httpContextAccessor);
        }

        public MediaModel GetMediaModelById(string id)
        {
            return _repository.GetMediaById(GuidHelper.StringToGuidConverter(id)) ??
                   throw new ApiError(HttpStatusCode.NotFound, $"Media with id {id} not found");
        }

        public MediaPublicDto RetryBuild(string id, ConnectedUser currentUser)
        {
            var media = GetMediaModelById(id);

            if (media.Organization.Users.All(u => u.Id != currentUser.Id))
            {
                throw new ApiError(HttpStatusCode.Unauthorized, $"You are not part of the media organization");
            }

            var mediaDto = ConstructMediaDto(media);

            try
            {
                _unityRmqClient.SendPayload(mediaDto);
            }
            catch (ApiError error)
            {
                throw new ApiError(error.StatusCode, JsonSerializer.Serialize(mediaDto));
            }

            return mediaDto;
        }

        private MediaPublicDto ConstructMediaDto(MediaModel media)
        {
            var dto = _mapper.Map(media, new MediaPublicDto());

            dto.Media = media.Type switch
            {
                Type.Type2D => _mapper.Map(_repository.Get2DMediaByMediaId(media.Id) ??
                                           throw new ApiError(HttpStatusCode.PartialContent,
                                               $"Media with id {media.Id} does not have a 2D media"),
                    new Media2DPublicDto()),
                Type.Type3D => _mapper.Map(_repository.Get3DMediaByMediaId(media.Id) ??
                                           throw new ApiError(HttpStatusCode.PartialContent,
                                               $"Media with id {media.Id} does not have a 3D media"),
                    new Media3DPublicDto()),
                _ => throw new ApiError(HttpStatusCode.PartialContent,
                    $"Media with id {media.Id} does not have media")
            };
            return dto;
        }

        private IList<TagModel> ResolveTags(IEnumerable<string> tagNames)
        {
            return (from tagName in tagNames
                where !string.IsNullOrEmpty(tagName)
                select _tagBusiness.GetTagModelByName(tagName)).ToList();
        }

        private void SaveMedia2D(MediaCreationDto mediaCDto, MediaModel media)
        {
            var media2DCreationDto = _mapper.Map(mediaCDto, new Media2DCreationDto());
            var media2DModel = _mapper.Map(media2DCreationDto, new Media2DModel());

            if (media2DCreationDto.Texture == null ||
                media2DCreationDto.NormalMap == null ||
                media2DCreationDto.AspectRatio == 0)
            {
                throw new ApiError(HttpStatusCode.BadRequest, "2D media not valid");
            }

            var assetsDir = $"/assets/{media.Id.ToString()}";

            // Create media dir if not exists
            if (!Directory.Exists(assetsDir))
            {
                Directory.CreateDirectory(assetsDir);
            }

            // Saving texture
            using (var fileStream =
                new FileStream(
                    $"{assetsDir}/texture{Path.GetExtension(media2DCreationDto.Texture.FileName)}",
                    FileMode.Create))
            {
                media2DCreationDto.Texture.CopyTo(fileStream);
                media2DModel.TextureLink = _uriHelper.UriBuilder(fileStream.Name);
            }

            // Saving normal map
            using (var fileStream =
                new FileStream(
                    $"{assetsDir}/normal_map{Path.GetExtension(media2DCreationDto.NormalMap.FileName)}",
                    FileMode.Create))
            {
                media2DCreationDto.NormalMap.CopyTo(fileStream);
                media2DModel.NormalMapLink = _uriHelper.UriBuilder(fileStream.Name);
            }

            media2DModel.Media = media;
            var _ = _repository.AddNew2DMedia(media2DModel) ?? throw new ApiError(HttpStatusCode.Conflict,
                $"Cannot save 2D media for media with id {media.Id}");
        }

        private void SaveMedia3D(MediaCreationDto mediaCDto, MediaModel media)
        {
            var media3DCreationDto = _mapper.Map(mediaCDto, new Media3DCreationDto());
            var media3DModel = _mapper.Map(media3DCreationDto, new Media3DModel());

            if (media3DCreationDto.Model == null ||
                media3DCreationDto.NormalMap == null ||
                media3DCreationDto.Width == 0 ||
                media3DCreationDto.Height == 0 ||
                media3DCreationDto.Depth == 0 ||
                media3DCreationDto.Texture == null)
            {
                throw new ApiError(HttpStatusCode.BadRequest, "3D media not valid");
            }

            var assetsDir = $"/assets/{media.Id.ToString()}";

            // Create media dir if not exists
            if (!Directory.Exists(assetsDir))
            {
                Directory.CreateDirectory(assetsDir);
            }

            // Saving texture
            using (var fileStream =
                new FileStream(
                    $"{assetsDir}/texture{Path.GetExtension(media3DCreationDto.Texture.FileName)}",
                    FileMode.Create))
            {
                media3DCreationDto.Texture.CopyTo(fileStream);
                media3DModel.TextureLink = _uriHelper.UriBuilder(fileStream.Name);
            }

            // Saving model
            using (var fileStream =
                new FileStream(
                    $"{assetsDir}/model{Path.GetExtension(media3DCreationDto.Texture.FileName)}",
                    FileMode.Create))
            {
                media3DCreationDto.NormalMap.CopyTo(fileStream);
                media3DModel.ModelLink = _uriHelper.UriBuilder(fileStream.Name);
            }

            // Saving normal map
            using (var fileStream =
                new FileStream(
                    $"{assetsDir}/nomal_map{Path.GetExtension(media3DCreationDto.NormalMap.FileName)}",
                    FileMode.Create))
            {
                media3DCreationDto.NormalMap.CopyTo(fileStream);
                media3DModel.NormalMapLink = _uriHelper.UriBuilder(fileStream.Name);
            }

            media3DModel.Media = media;
            var _ = _repository.AddNew3DMedia(media3DModel) ?? throw new ApiError(HttpStatusCode.Conflict,
                $"Cannot save 3D media for media with id {media.Id}");
        }

        public MediaPublicDto GetMediaById(string id, ConnectedUser currentUser)
        {
            var media = GetMediaModelById(id);

            if (media.Organization.Users.All(u => u.Id != currentUser.Id))
            {
                throw new ApiError(HttpStatusCode.Unauthorized, $"You are not part of the media organization");
            }

            return ConstructMediaDto(media);
        }

        public (int page, int pageSize, int totalItemCount, IList<MediaPublicDto> medias) GetMedias(PagingDto paging,
            IList<string> tagNames,
            string orgId,
            ConnectedUser currentUser)
        {
            var org = _organizationBusiness.GetOrganizationModelById(GuidHelper.StringToGuidConverter(orgId));

            if (!_organizationBusiness.IsUserInOrganization(org.Id, currentUser.Id))
            {
                throw new ApiError(HttpStatusCode.Unauthorized,
                    "You cannot add a media to an organization you are not part of");
            }

            paging = PagingHelper.Check(paging);

            if (tagNames.Count > 0)
            {
                var (mediaModels, totalItemCount) = _repository.GetMediasByTags((paging.Page - 1) * paging.PageSize,
                    paging.PageSize, org.Id, currentUser.Id, ResolveTags(tagNames));
                return (paging.Page, paging.PageSize, totalItemCount,
                    _mapper.Map(mediaModels, new List<MediaPublicDto>()));
            }
            else
            {
                var (mediaModels, totalItemCount) = _repository.GetMediasByOrganizationId(
                    (paging.Page - 1) * paging.PageSize,
                    paging.PageSize, org.Id, currentUser.Id);
                return (paging.Page, paging.PageSize, totalItemCount,
                    _mapper.Map(mediaModels, new List<MediaPublicDto>()));
            }
        }

        public MediaPublicDto AddNewMedia(MediaCreationDto newMedia, ConnectedUser currentUser)
        {
            var media = _mapper.Map(newMedia, new MediaModel());

            if (!_organizationBusiness.IsUserInOrganization(GuidHelper.StringToGuidConverter(newMedia.OrganizationId),
                currentUser.Id))
            {
                throw new ApiError(HttpStatusCode.Unauthorized,
                    "You cannot add a media to an organization you are not part of");
            }

            media.Tags = ResolveTags(newMedia.TagName);
            media.Organization =
                _organizationBusiness.GetOrganizationModelById(
                    GuidHelper.StringToGuidConverter(newMedia.OrganizationId));
            media.State = MediaStateEnum.Pending;
            media.StateMessage = "Awaiting processing";
            var savedMedia = _repository.AddNewMedia(media);

            switch (newMedia.Type)
            {
                case Type.Type2D:
                    SaveMedia2D(newMedia, savedMedia);
                    break;
                case Type.Type3D:
                    SaveMedia3D(newMedia, savedMedia);
                    break;
                default:
                    throw new ApiError(HttpStatusCode.BadRequest, "Media type not valid");
            }

            var mediaDto = ConstructMediaDto(savedMedia);

            try
            {
                _unityRmqClient.SendPayload(mediaDto);
            }
            catch (ApiError error)
            {
                throw new ApiError(error.StatusCode, JsonSerializer.Serialize(mediaDto));
            }

            return mediaDto;
        }

        public MediaPublicDto UpdateMediaById(string id, MediaUpdateDto updatedMedia, ConnectedUser currentUser)
        {
            var media = GetMediaModelById(id);

            if (media.Organization.Users.All(u => u.Id != currentUser.Id))
            {
                throw new ApiError(HttpStatusCode.Unauthorized, $"You are not part of the media organization");
            }

            if (media.Tags.Count > 0)
            {
                media.Tags = ResolveTags(updatedMedia.TagName);
            }

            if (!string.IsNullOrEmpty(updatedMedia.Name))
            {
                media.Name = updatedMedia.Name;
            }

            return _mapper.Map(_repository.UpdateMedia(media), new MediaPublicDto());
        }

        public MediaPublicDto GetEngineMediaById(string id, ConnectedUser currentUser, Engine engine)
        {
            var media = GetMediaModelById(id);

            if (media.Organization.Users.All(u => u.Id != currentUser.Id))
            {
                throw new ApiError(HttpStatusCode.Unauthorized, $"You are not part of the media organization");
            }

            var dto = _mapper.Map(media, new MediaPublicDto());

            dto.Media = engine switch
            {
                Engine.Unity => GetMediaUnityPublicDtoByMediaId(media.Id.ToString()),
                _ => throw new ApiError(HttpStatusCode.PartialContent,
                    $"Media with id {media.Id} does not have specified media")
            };
            return dto;
        }

        public MediaUnityPublicDto GetEngineMedia(MediaQueryFilters filters)
        {
            var mediaModel = filters.Engine switch
            {
                Engine.Unity => _repository.GetUnityMediaByFilters(filters) ??
                                throw new ApiError(HttpStatusCode.NotFound, "No Media Found"),
                _ => throw new ApiError(HttpStatusCode.Unused, "Not implemented")
            };
            return _mapper.Map(mediaModel, new MediaUnityPublicDto());
        }

        private MediaUnityPublicDto GetMediaUnityPublicDtoByMediaId(string mediaId)
        {
            var mediaUnity = _repository.GetUnityMediaByMediaId(GuidHelper.StringToGuidConverter(mediaId)) ??
                             throw new ApiError(HttpStatusCode.PartialContent,
                                 $"Media with id {mediaId} does not have an Unity media");

            return _mapper.Map(mediaUnity, new MediaUnityPublicDto());
        }

        public void DeleteMediaById(string id, ConnectedUser currentUser)
        {
            var media = GetMediaModelById(id);

            if (media.Organization.Users.All(u => u.Id != currentUser.Id))
            {
                throw new ApiError(HttpStatusCode.Unauthorized, $"You are not part of the media organization");
            }

            _repository.DeleteMedia(media);
        }

        public MediaUnityPublicDto AddNewMediaUnity(MediaUnityCreationDto newMediaUnity, string mediaId)
        {
            var mediaUnityModel = new MediaUnityModel();

            var media = GetMediaModelById(mediaId);

            if (newMediaUnity.AssetBundle == null && newMediaUnity.State == 0)
            {
                throw new ApiError(HttpStatusCode.BadRequest, "You need to specify at least an AssetBundle or a State");
            }

            if (_repository.GetUnityMediaById(media.Id) != null)
            {
                throw new ApiError(HttpStatusCode.Conflict, $"Media with id ${media.Id} already have an unity media");
            }

            // Saving asset bundle
            if (newMediaUnity.AssetBundle != null)
            {
                var assetsDir = $"/assets/{mediaId}";

                // Create media dir if not exists
                if (!Directory.Exists(assetsDir))
                {
                    Directory.CreateDirectory(assetsDir);
                }

                using var fileStream =
                    new FileStream(
                        $"{assetsDir}/unity{Path.GetExtension(newMediaUnity.AssetBundle.FileName)}",
                        FileMode.Create);
                newMediaUnity.AssetBundle.CopyTo(fileStream);
                mediaUnityModel.AssetBundleLink = _uriHelper.UriBuilder(fileStream.Name);
            }

            if (newMediaUnity.State != 0)
            {
                mediaUnityModel.State = newMediaUnity.State;
                mediaUnityModel.StateMessage = newMediaUnity.StateMessage;
            }
            else
            {
                mediaUnityModel.State = MediaStateEnum.Processed;
                mediaUnityModel.StateMessage = "Unity media processed";
            }

            UpdateMediaStateFromEngine(new MediaState
            {
                State = mediaUnityModel.State,
                Message = mediaUnityModel.StateMessage
            }, mediaId, "unity");
            mediaUnityModel.Media = media;
            var mediaUnityModelSaved = _repository.AddNewUnityMedia(mediaUnityModel) ?? throw new ApiError(
                HttpStatusCode.Conflict,
                $"Cannot save Unity media for media with id {media.Id}");

            return _mapper.Map(mediaUnityModelSaved, new MediaUnityPublicDto());
        }

        private void UpdateMediaStateFromEngine(MediaState newState, string mediaId, string engine)
        {
            var media = GetMediaModelById(mediaId);

            if (newState.State != MediaStateEnum.Invalid && newState.State != MediaStateEnum.Processing &&
                newState.State != MediaStateEnum.Processed) return;

            media.State = newState.State;
            media.StateMessage = $"|{engine}:{newState.Message}|";
            _repository.UpdateMedia(media);
        }

        public MediaUnityPublicDto UpdateMediaUnity(MediaUnityUpdateDto updatedUnityMedia, string mediaId)
        {
            var _ = GetMediaModelById(mediaId);

            var mediaUnity = _repository.GetUnityMediaById(GuidHelper.StringToGuidConverter(mediaId)) ??
                             throw new ApiError(HttpStatusCode.NotFound,
                                 $"Media with id ${mediaId} does not have a unity media");

            // Saving asset bundle
            if (updatedUnityMedia.AssetBundle != null)
            {
                var assetsDir = $"/assets/{mediaId}";

                // Create media dir if not exists
                if (!Directory.Exists(assetsDir))
                {
                    Directory.CreateDirectory(assetsDir);
                }

                using var fileStream =
                    new FileStream(
                        $"{assetsDir}/unity{Path.GetExtension(updatedUnityMedia.AssetBundle.FileName)}",
                        FileMode.Create);
                updatedUnityMedia.AssetBundle.CopyTo(fileStream);
                mediaUnity.AssetBundleLink = _uriHelper.UriBuilder(fileStream.Name);
            }

            if (updatedUnityMedia.State != null)
            {
                mediaUnity.State = updatedUnityMedia.State.State;
                mediaUnity.StateMessage = updatedUnityMedia.State.Message;
                UpdateMediaStateFromEngine(updatedUnityMedia.State, mediaId, "unity");
            }

            var mediaUnitySaved = _repository.UpdateUnityMedia(mediaUnity) ?? throw new ApiError(
                HttpStatusCode.Conflict,
                $"Cannot save Unity media for media with id {mediaUnity.Media.Id}");

            return _mapper.Map(mediaUnitySaved, new MediaUnityPublicDto());
        }

        public MediaPublicDto UpdateMediaState(MediaState newState, string mediaId)
        {
            var media = _repository.GetMediaById(GuidHelper.StringToGuidConverter(mediaId)) ??
                        throw new ApiError(HttpStatusCode.NotFound, $"media with ID ${mediaId} not found");
            media.State = newState.State;
            media.StateMessage = newState.Message;
            return _mapper.Map(_repository.UpdateMedia(media), new MediaPublicDto());
        }
    }
}