using System;
using System.Collections.Generic;
using System.Linq;
using api.Contexts;
using api.Models.Advertisement;
using Microsoft.EntityFrameworkCore;

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
            return _context.Advertisement
                .Include(v => v.Campaign)
                .ThenInclude(g => g.Organization)
                .Include(a => a.Media)
                .SingleOrDefault(a => a.Id == id);
        }

        public (List<AdvertisementModel>, int totalItemCount) GetAdvertisements(int offset, int limit, AdvertisementFiltersDto filters)
        {
            IQueryable<AdvertisementModel> query = _context.Advertisement.OrderBy(a => a.DateCreation);

            query = query.Include(advertisement => advertisement.Media);

            if (filters.OrganizationId != Guid.Empty)
            {
                query = query.Where(o => o.Campaign.Organization.Id == filters.OrganizationId);
            }

            if (filters.CampaignId != Guid.Empty)
            {
                query = query.Where(o => o.Campaign.Id == filters.CampaignId);
            }

            return (query.Skip(offset).Take(limit).ToList(), query.Count());
        }

        public AdvertisementModel UpdateAdvertisement(AdvertisementModel advertisement)
        {
            _context.Advertisement.Update(advertisement);
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