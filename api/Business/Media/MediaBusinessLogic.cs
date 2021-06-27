using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using api.Business.Organization;
using api.Business.Tag;
using api.Contexts;
using api.Enums.Media;
using api.Errors;
using api.Helpers;
using api.Models.Media;
using api.Models.Media._2D;
using api.Models.Media._3D;
using api.Models.Tag;
using api.Repositories.Media;
using AutoMapper;
using Type = api.Enums.Media.Type;

namespace api.Business.Media
{
    public class MediaBusinessLogic : IMediaBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly IMediaRepository _repository;
        private readonly IOrganizationBusinessLogic _organizationBusiness;
        private readonly ITagBusinessLogic _tagBusiness;

        public MediaBusinessLogic(ApiContext context, IMapper mapper, IOrganizationBusinessLogic organizationBusiness,
            ITagBusinessLogic tagBusiness)
        {
            _repository = new MediaRepository(context);
            _mapper = mapper;
            _organizationBusiness = organizationBusiness;
            _tagBusiness = tagBusiness;
        }

        private MediaModel GetMediaModelById(string id)
        {
            return _repository.GetMediaById(GuidHelper.StringToGuidConverter(id)) ??
                   throw new ApiError(HttpStatusCode.NotFound, $"Media with id {id} not found");
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

        public (int page, int pageSize, int maxPage, IList<MediaPublicDto> medias) GetMedias(PagingDto paging,
            string versionId,
            ConnectedUser currentUser)
        {
            throw new NotImplementedException();
        }

        public MediaPublicDto AddNewMedia(MediaCreationDto newMedia, ConnectedUser currentUser)
        {
            var media = _mapper.Map(newMedia, new MediaModel());

            if (!_organizationBusiness.IsUserInOrganization(GuidHelper.StringToGuidConverter(newMedia.OrgId),
                currentUser.Id))
            {
                throw new ApiError(HttpStatusCode.Unauthorized,
                    "You cannot add a media to an organization you are not part of");
            }

            media.Tags = ResolveTags(newMedia.TagName);
            media.Organization = _organizationBusiness.GetOrganizationModelById(newMedia.OrgId);
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

            return ConstructMediaDto(savedMedia);
        }

        public MediaPublicDto UpdateMediaById(string id, MediaUpdateDto updatedMedia, ConnectedUser currentUser)
        {
            var media = GetMediaModelById(id);

            if (media.Organization.Users.All(u => u.Id != currentUser.Id))
            {
                throw new ApiError(HttpStatusCode.Unauthorized, $"You are not part of the media organization");
            }

            media.Tags = ResolveTags(updatedMedia.TagName);

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
                Engine.Unity => _repository.GetUnityMediaByMediaId(media.Id) ??
                                throw new ApiError(HttpStatusCode.PartialContent,
                                    $"Media with id {media.Id} does not have an Unity media"),
                _ => throw new ApiError(HttpStatusCode.PartialContent,
                    $"Media with id {media.Id} does not have specified media")
            };
            return dto;
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
    }
}