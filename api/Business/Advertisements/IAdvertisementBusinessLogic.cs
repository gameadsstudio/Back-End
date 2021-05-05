using System.Collections.Generic;
using System.Security.Claims;
using api.Helpers;
using api.Models.Advertisement;

namespace api.Business.Advertisements
{
    public interface IAdvertisementBusinessLogic
    {
        AdvertisementPublicDto GetAdvertisementById(string id, Claim currentUser);
        AdvertisementModel GetAdvertisementModelById(string id);
        (int page, int pageSize, int maxPage, List<AdvertisementPublicDto> advertisements) GetAdvertisements(PagingDto paging);
        AdvertisementPublicDto AddNewAdvertisement(AdvertisementCreationDto newAdvertisement);
        AdvertisementPublicDto UpdateAdvertisementById(string id, AdvertisementUpdateDto updatedAdvertisement, Claim currentUser);
        void DeleteAdvertisementById(string id, Claim currentUser);
    }
}