using System.Security.Claims;
using api.Helpers;
using api.Models.Advertisement;

namespace api.Business.Advertisements
{
    public interface IAdvertisementBusinessLogic
    {
        AdvertisementModel GetAdvertisementById(string id, Claim currentUser);
        (int page, int pageSize, int maxPage, AdvertisementModel[] advertisements) GetAdvertisements(PagingDto paging);
        AdvertisementModel AddNewAdvertisement(AdvertisementCreationDto newAdvertisement);
        AdvertisementModel UpdateAdvertisementById(string id, AdvertisementUpdateDto updatedAdvertisement, Claim currentUser);
        int DeleteAdvertisementById(string id, Claim currentUser);
    }
}