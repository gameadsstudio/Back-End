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
                   throw new ApiError(HttpStatusCode.NotFound, $"Media with id ${id} not found");
        }

        private MediaPublicDto ConstructMediaDto(MediaModel media)
        {
            var dto = _mapper.Map(media, new MediaPublicDto());

            dto.Media = media.Type switch
            {
                Type.Type2D => _repository.Get2DMediaByMediaId(media.Id) ??
                               throw new ApiError(HttpStatusCode.ExpectationFailed,
                                   $"Media with id ${media.Id} does not have a 2D media"),
                Type.Type3D => _repository.Get3DMediaByMediaId(media.Id) ??
                               throw new ApiError(HttpStatusCode.ExpectationFailed,
                                   $"Media with id ${media.Id} does not have a 3D media"),
                _ => throw new ApiError(HttpStatusCode.ExpectationFailed,
                    $"Media with id ${media.Id} does not have media")
            };
            return dto;
        }

        private IList<TagModel> ResolveTags(IEnumerable<string> tagNames)
        {
            return (from tagName in tagNames
                where !string.IsNullOrEmpty(tagName)
                select _tagBusiness.GetTagModelByName(tagName)).ToList();
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
            throw new System.NotImplementedException();
        }

        public MediaPublicDto AddNewMedia(MediaCreationDto newMedia, ConnectedUser currentUser)
        {
            var media = _mapper.Map(newMedia, new MediaModel());

            if (!_organizationBusiness.IsUserInOrganization(GuidHelper.StringToGuidConverter(newMedia.OrgId),
                currentUser.Id))
            {
                throw new ApiError(HttpStatusCode.Unauthorized,
                    $"You cannot add a media to an organization you are not part of");
            }

            media.Tags = ResolveTags(newMedia.TagName);
            media.Organization = _organizationBusiness.GetOrganizationModelById(newMedia.OrgId);

            return _mapper.Map(_repository.AddNewMedia(media), new MediaPublicDto());
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
                                throw new ApiError(HttpStatusCode.ExpectationFailed,
                                    $"Media with id ${media.Id} does not have an Unity media"),
                _ => throw new ApiError(HttpStatusCode.ExpectationFailed,
                    $"Media with id ${media.Id} does not have specified media")
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