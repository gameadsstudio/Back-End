using System;
using System.Collections.Generic;
using api.Helpers;
using api.Models.Advertisement;

namespace api.Business.Advertisements
{
    public interface IAdvertisementBusinessLogic
    {
        AdvertisementPublicDto GetAdvertisementById(Guid id, ConnectedUser currentUser);
        AdvertisementModel GetAdvertisementModelById(Guid id);
        (int page, int pageSize, int totalItemCount, List<AdvertisementPublicDto> advertisements) GetAdvertisements(PagingDto paging, AdvertisementFiltersDto filters, ConnectedUser currentUser);
        AdvertisementPublicDto AddNewAdvertisement(AdvertisementCreationDto newAdvertisement, ConnectedUser currentUser);
        AdvertisementPublicDto UpdateAdvertisementById(Guid id, AdvertisementUpdateDto updatedAdvertisement, ConnectedUser currentUser);
        void DeleteAdvertisementById(Guid id, ConnectedUser currentUser);
    }
}