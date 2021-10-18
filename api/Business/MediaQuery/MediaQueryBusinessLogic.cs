using System;
using api.Business.AdContainer;
using api.Business.Media;
using api.Enums.Media;
using api.Helpers;
using api.Models.Media;

namespace api.Business.MediaQuery
{
    public class MediaQueryBusinessLogic : IMediaQueryBusinessLogic
    {
        private readonly IMediaBusinessLogic _mediaBusinessLogic;
        private readonly IAdContainerBusinessLogic _adContainerBusinessLogic;

        public MediaQueryBusinessLogic(IMediaBusinessLogic mediaBusinessLogic, IAdContainerBusinessLogic adContainerBusinessLogic)
        {
            _mediaBusinessLogic = mediaBusinessLogic;
            _adContainerBusinessLogic = adContainerBusinessLogic;
        }

        public MediaPublicDto GetMedia(string adContainerId, Engine engine, ConnectedUser currentUser)
        {
            var adContainer = _adContainerBusinessLogic.GetAdContainerById(adContainerId, currentUser);

            throw new NotImplementedException();
        }
    }
}