using System;
using api.Models.Advertisement;

namespace api.Repositories.Advertisement
{
    public interface IAdvertisementRepository
    {
        AdvertisementModel AddNewAdvertisement(AdvertisementModel user);
        AdvertisementModel GetAdvertisementById(Guid id);
        AdvertisementModel[] GetAdvertisements(int offset, int limit);
        AdvertisementModel UpdateAdvertisement(AdvertisementModel updatedUser);
        void DeleteAdvertisement(AdvertisementModel user);
        int CountAdvertisements();
    }
}