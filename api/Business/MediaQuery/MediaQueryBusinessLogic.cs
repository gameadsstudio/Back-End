using System.Net;
using api.Business.AdContainer;
using api.Business.Media;
using api.Business.Organization;
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
            IAdContainerBusinessLogic adContainerBusinessLogic, IOrganizationBusinessLogic organizationBusinessLogic,
            IMapper mapper)
        {
            _mediaBusinessLogic = mediaBusinessLogic;
            _adContainerBusinessLogic = adContainerBusinessLogic;
            _mapper = mapper;
        }

        public object GetMedia(string adContainerId, Engine engine, ConnectedUser currentUser)
        {
            var adContainer = _adContainerBusinessLogic.GetAdContainerModelById(adContainerId);
            var media = _mediaBusinessLogic.GetEngineMedia(new MediaQueryFilters(engine, adContainer));

            if (media == null)
            {
                throw new ApiError(HttpStatusCode.NotFound, $"No media found for ad container ${adContainerId}");
            }
            
            return engine switch
            {
                Engine.Unity => _mapper.Map(media, new MediaUnityPublicDto()),
                _ => throw new ApiError(HttpStatusCode.NotImplemented, "Specified engine not implemented")
            };
        }
    }
}