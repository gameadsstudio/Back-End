using System;
using System.Collections.Generic;
using api.Business.Media;
using api.Helpers;
using api.Models.Media;

namespace api.Business.MediaQuery
{
    public class MediaQueryBusinessLogic : IMediaQueryBusinessLogic
    {
        private readonly IMediaBusinessLogic _mediaBusinessLogic;

        public MediaQueryBusinessLogic(IMediaBusinessLogic mediaBusinessLogic)
        {
            _mediaBusinessLogic = mediaBusinessLogic;
        }

        public MediaPublicDto GetMedia(PagingDto paging, IList<string> tagNames, string adContainerId, ConnectedUser currentUser)
        {
            throw new NotImplementedException();
        }
    }
}