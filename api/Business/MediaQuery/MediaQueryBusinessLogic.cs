using System.Net;
using api.Business.AdContainer;
using api.Business.Media;
using api.Enums.Media;
using api.Errors;
using api.Helpers;
using api.Models.Media;
using AutoMapper;
using Type = api.Enums.Media.Type;

namespace api.Business.MediaQuery
{
    public class MediaQueryBusinessLogic : IMediaQueryBusinessLogic
    {
        private readonly IMediaBusinessLogic _mediaBusinessLogic;
        private readonly IAdContainerBusinessLogic _adContainerBusinessLogic;
        private readonly IMapper _mapper;

        public MediaQueryBusinessLogic(IMediaBusinessLogic mediaBusinessLogic,
            IAdContainerBusinessLogic adContainerBusinessLogic,
            IMapper mapper)
        {
            _mediaBusinessLogic = mediaBusinessLogic;
            _adContainerBusinessLogic = adContainerBusinessLogic;
            _mapper = mapper;
        }

        public object GetMedia(string adContainerId, Engine engine, ConnectedUser currentUser)
        {
            var adContainer = _adContainerBusinessLogic.GetAdContainerModelById(adContainerId);

            var mediaIDs = adContainer.Type switch
            {
                Type.Type2D => _mediaBusinessLogic.GetMedia2DIds(adContainer.AspectRatio),
                Type.Type3D => _mediaBusinessLogic.GetMedia3DIds(adContainer.Width, adContainer.Height,
                    adContainer.Depth),
                _ => throw new MediaTypeNotSupportedError(),
            };
            
            var engineMedia = _mediaBusinessLogic.GetEngineMedia(new MediaQueryFilters(engine, mediaIDs, adContainer.Tags));

            if (engineMedia == null)
            {
                throw new MediaNotFoundForAdContainerError();
            }

            var media = _mediaBusinessLogic.GetMediaModelById(engineMedia.Media.Id.ToString());

            var result = engine switch
            {
                Engine.Unity => _mapper.Map(media, new MediaPublicDto()),
                _ => throw new MediaEngineNotImplementedError()
            };
            
            result.Media = engine switch
            {
                Engine.Unity => _mediaBusinessLogic.GetMediaUnityPublicDtoByMediaId(media.Id.ToString()),
                _ => throw new MediaNotSpecifiedError()
            };

            return result;
            
        }
    }
}