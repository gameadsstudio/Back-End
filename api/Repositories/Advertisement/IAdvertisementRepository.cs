using System;
using System.Collections.Generic;
using api.Models.Advertisement;

namespace api.Repositories.Advertisement
{
    public interface IAdvertisementRepository
    {
        AdvertisementModel AddNewAdvertisement(AdvertisementModel user);
        AdvertisementModel GetAdvertisementById(Guid id);
        (List<AdvertisementModel>, int totalItemCount) GetAdvertisements(int offset, int limit, AdvertisementFiltersDto filters);
        AdvertisementModel UpdateAdvertisement(AdvertisementModel updatedUser);
        int DeleteAdvertisement(AdvertisementModel user);
    }
}