using System.Security.Claims;
using api.Helpers;
using api.Models.Advertisement;

namespace api.Business.Advertisements
{
    public class AdvertisementBusinessLogic : IAdvertisementBusinessLogic
    {
        public AdvertisementModel GetAdvertisementById(string id, Claim currentUser)
        {
            throw new System.NotImplementedException();
        }

        public (int page, int pageSize, int maxPage, AdvertisementModel[] advertisements) GetAdvertisements(PagingDto paging)
        {
            throw new System.NotImplementedException();
        }

        public AdvertisementModel AddNewAdvertisement(AdvertisementCreationModel newAdvertisement)
        {
            throw new System.NotImplementedException();
        }

        public AdvertisementModel UpdateAdvertisementById(string id, AdvertisementUpdateModel updatedAdvertisement, Claim currentUser)
        {
            throw new System.NotImplementedException();
        }

        public int DeleteAdvertisementById(string id, Claim currentUser)
        {
            throw new System.NotImplementedException();
        }
    }
}