using System;
using System.Net;
using api.Business.AdContainer;
using api.Business.Media;
using api.Enums.Media;
using api.Errors;
using api.Helpers;
using api.Models.Media;
using api.Models.Media.Engine.Unity;
using AutoMapper;

namespace api.Business.MediaQuery
{
    public class MediaQueryBusinessLogic : IMediaQueryBusinessLogic
    {
        private readonly IMediaBusinessLogic _mediaBusinessLogic;
        private readonly IAdContainerBusinessLogic _adContainerBusinessLogic;
        private readonly IMapper _mapper;

        public MediaQueryBusinessLogic(IMediaBusinessLogic mediaBusinessLogic,
            IAdContainerBusinessLogic adContainerBusinessLogic, IMapper mapper)
        {
            _mediaBusinessLogic = mediaBusinessLogic;
            _adContainerBusinessLogic = adContainerBusinessLogic;
            _mapper = mapper;
        }

        public object GetMedia(string adContainerId, Engine engine, ConnectedUser currentUser)
        {
            var adContainer = _adContainerBusinessLogic.GetAdContainerById(adContainerId, currentUser);
            var medias =
                _mediaBusinessLogic.GetEngineMedias(new MediaQueryFilters(adContainer.Tags, engine, adContainer.Type));

            if (medias.Count == 0)
            {
                throw new ApiError(HttpStatusCode.NotFound, $"No media found for ad container ${adContainerId}");
            }
            var rnd = new Random();
            var r = rnd.Next(medias.Count);

            return engine switch
            {
                Engine.Unity => _mapper.Map(medias[r], new MediaUnityPublicDto()),
                _ => throw new ApiError(HttpStatusCode.NotImplemented, "Specified engine not implemented")
            };
        }
    }
}