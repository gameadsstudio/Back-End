using System;
using api.Models.Advertisement;

namespace api.Repositories.Advertisement
{
    public class AdvertisementRepository : IAdvertisementRepository
    {
        public AdvertisementModel AddNewAdvertisement(AdvertisementModel user)
        {
            throw new NotImplementedException();
        }

        public AdvertisementModel GetAdvertisementById(Guid id)
        {
            throw new NotImplementedException();
        }

        public AdvertisementModel[] GetAdvertisements(int offset, int limit)
        {
            throw new NotImplementedException();
        }

        public AdvertisementModel UpdateAdvertisement(AdvertisementModel updatedUser)
        {
            throw new NotImplementedException();
        }

        public int DeleteAdvertisement(AdvertisementModel user)
        {
            throw new NotImplementedException();
        }

        public int CountAdvertisements()
        {
            throw new NotImplementedException();
        }
    }
}