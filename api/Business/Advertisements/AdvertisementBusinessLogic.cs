using System;
using System.Collections.Generic;
using System.Net;
using api.Business.Campaign;
using api.Business.Media;
using api.Business.Organization;
using api.Contexts;
using api.Enums.User;
using api.Errors;
using api.Helpers;
using api.Models.Advertisement;
using api.Repositories.Advertisement;
using AutoMapper;

namespace api.Business.Advertisements
{
    public class AdvertisementBusinessLogic : IAdvertisementBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly IAdvertisementRepository _repository;

        private readonly IOrganizationBusinessLogic _organizationBusinessLogic;
        private readonly ICampaignBusinessLogic _campaignBusinessLogic;
        private readonly IMediaBusinessLogic _mediaBusinessLogic;

        public AdvertisementBusinessLogic(ApiContext context, IMapper mapper,
            IOrganizationBusinessLogic organizationBusinessLogic, ICampaignBusinessLogic campaignBusinessLogic,
            IMediaBusinessLogic mediaBusinessLogic)
        {
            _repository = new AdvertisementRepository(context);
            _organizationBusinessLogic = organizationBusinessLogic;
            _campaignBusinessLogic = campaignBusinessLogic;
            _mediaBusinessLogic = mediaBusinessLogic;
            _mapper = mapper;
        }

        public AdvertisementPublicDto GetAdvertisementById(Guid id, ConnectedUser currentUser)
        {
            var advertisement = GetAdvertisementModelById(id);

            if (!_organizationBusinessLogic.IsUserInOrganization(advertisement.Campaign.Organization.Id,
                currentUser.Id) && currentUser.Role != UserRole.User)
            {
                throw new AdvertisementInsufficientRightsError();
            }

            return _mapper.Map(advertisement, new AdvertisementPublicDto());
        }

        public AdvertisementModel GetAdvertisementModelById(Guid id)
        {
            var advertisement = _repository.GetAdvertisementById(id);

            if (advertisement == null)
            {
                throw new AdvertisementNotFoundError();
            }

            return advertisement;
        }

        public (int page, int pageSize, int totalItemCount, List<AdvertisementPublicDto> advertisements)
            GetAdvertisements(PagingDto paging, AdvertisementFiltersDto filters, ConnectedUser currentUser)
        {
            if (!_organizationBusinessLogic.IsUserInOrganization(filters.OrganizationId, currentUser.Id) &&
                currentUser.Role != UserRole.User)
            {
                throw new AdvertisementInsufficientRightsError();
            }

            paging = PagingHelper.Check(paging);
            var (advertisements, totalItemCount) =
                _repository.GetAdvertisements((paging.Page - 1) * paging.PageSize, paging.PageSize, filters);
            return (paging.Page, paging.PageSize, totalItemCount,
                _mapper.Map(advertisements, new List<AdvertisementPublicDto>()));
        }

        public AdvertisementPublicDto AddNewAdvertisement(AdvertisementCreationDto newAdvertisement,
            ConnectedUser currentUser)
        {
            var campaign = _campaignBusinessLogic.GetCampaignModelById(newAdvertisement.CampaignId);

            if (!_organizationBusinessLogic.IsUserInOrganization(campaign.Organization.Id, currentUser.Id) &&
                currentUser.Role != UserRole.User)
            {
                throw new AdvertisementInsufficientRightsError();
            }

            var advertisement = _mapper.Map(newAdvertisement, new AdvertisementModel());

            advertisement.Campaign = campaign;

            if (newAdvertisement.MediaId != Guid.Empty)
            {
                var mediaDto = _mediaBusinessLogic.GetMediaById(newAdvertisement.MediaId.ToString(), currentUser);
                var media = _mediaBusinessLogic.GetMediaModelById(mediaDto.Id.ToString());
                advertisement.Media = media;
            }
            
            if (advertisement.AgeMin == 0)
            {
                advertisement.AgeMin = campaign.AgeMin;
            }

            if (advertisement.AgeMax == 0)
            {
                advertisement.AgeMax = campaign.AgeMax;
            }

            return _mapper.Map(_repository.AddNewAdvertisement(advertisement), new AdvertisementPublicDto());
        }

        public AdvertisementPublicDto UpdateAdvertisementById(Guid id, AdvertisementUpdateDto updatedAdvertisement,
            ConnectedUser currentUser)
        {
            var advertisement = GetAdvertisementModelById(id);

            if (!_organizationBusinessLogic.IsUserInOrganization(advertisement.Campaign.Organization.Id,
                currentUser.Id) && currentUser.Role != UserRole.User)
            {
                throw new AdvertisementInsufficientRightsError();
            }

            if (updatedAdvertisement.MediaId != Guid.Empty)
            {
                var media = _mediaBusinessLogic.GetMediaModelById(updatedAdvertisement.MediaId.ToString());
                if (media.Organization.Id != advertisement.Campaign.Organization.Id)
                {
                    throw new AdvertisementInsufficientRightsError();
                }

                advertisement.Media = media;
            }

            advertisement = _mapper.Map(updatedAdvertisement, advertisement);

            if (updatedAdvertisement.MediaId == Guid.Empty)
            {
                advertisement.Media = null;
            }

            return _mapper.Map(_repository.UpdateAdvertisement(advertisement), new AdvertisementPublicDto());
        }

        public void DeleteAdvertisementById(Guid id, ConnectedUser currentUser)
        {
            var advertisement = GetAdvertisementModelById(id);

            if (!_organizationBusinessLogic.IsUserInOrganization(advertisement.Campaign.Organization.Id,
                currentUser.Id) && currentUser.Role != UserRole.User)
            {
                throw new AdvertisementInsufficientRightsError();
            }

            _repository.DeleteAdvertisement(advertisement);
        }
    }
}