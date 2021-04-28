using System;
using System.Linq;
using api.Contexts;
using api.Models.Advertisement;

namespace api.Repositories.Advertisement
{
    public class AdvertisementRepository : IAdvertisementRepository
    {
        private readonly ApiContext _context;

        public AdvertisementRepository(ApiContext context)
        {
            _context = context;
        }

        public AdvertisementModel AddNewAdvertisement(AdvertisementModel advertisement)
        {
            _context.Advertisement.Add(advertisement);
            _context.SaveChanges();
            return advertisement;
        }

        public AdvertisementModel GetAdvertisementById(Guid id)
        {
            return _context.Advertisement.SingleOrDefault(a => a.Id == id);
        }

        public AdvertisementModel[] GetAdvertisements(int offset, int limit)
        {
            return _context.Advertisement.OrderBy(p => p.Id)
                .Skip(offset)
                .Take(limit)
                .ToArray();
        }

        public AdvertisementModel UpdateAdvertisement(AdvertisementModel advertisement)
        {
            _context.Advertisement.Add(advertisement);
            _context.SaveChanges();
            return advertisement;
        }

        public int DeleteAdvertisement(AdvertisementModel advertisement)
        {
            _context.Advertisement.Remove(advertisement);
            return _context.SaveChanges();
        }

        public int CountAdvertisements()
        {
            return _context.Advertisement.Count();
        }
    }
}