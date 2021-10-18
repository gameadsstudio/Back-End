using System;
using api.Business.AdContainer;
using api.Business.Media;
using api.Enums.Media;
using api.Helpers;
using api.Models.Media;
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

        public MediaPublicDto GetMedia(string adContainerId, Engine engine, ConnectedUser currentUser)
        {
            var adContainer = _adContainerBusinessLogic.GetAdContainerById(adContainerId, currentUser);
            var medias = _mediaBusinessLogic.GetEngineMedias(currentUser,
                new MediaQueryFilters(adContainer.Tags, engine, adContainer.Type));

            var rnd = new Random();
            var r = rnd.Next(medias.Count);

            return _mapper.Map(medias[r], new MediaPublicDto());
        }
    }
}