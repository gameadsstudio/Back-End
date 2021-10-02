using System;
using System.Collections.Generic;
using System.Linq;
using api.Contexts;
using api.Enums.Campaign;
using api.Models.Campaign;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories.Campaign
{
    public class CampaignRepository : ICampaignRepository
    {
        private readonly ApiContext _context;

        public CampaignRepository(ApiContext context)
        {
            _context = context;
        }

        public CampaignModel AddNewCampaign(CampaignModel campaign)
        {
            _context.Campaign.Add(campaign);
            _context.SaveChanges();
            return campaign;
        }

        public CampaignModel UpdateCampaign(CampaignModel campaign)
        {
            _context.Update(campaign);
            _context.SaveChanges();
            return campaign;
        }

        public int DeleteCampaign(CampaignModel campaign)
        {
            _context.Campaign.Remove(campaign);
            return _context.SaveChanges();
        }

        public CampaignModel GetCampaignById(Guid id)
        {
            return _context.Campaign.Include(x => x.Organization).SingleOrDefault(campaign => campaign.Id == id);
        }

        public (IList<CampaignModel> campaigns, int count) GetOrganizationCampaigns(Guid id, int offset, int limit)
        {
            var query = _context.Campaign.OrderByDescending(campaign => campaign.DateCreation)
                .Where(campaign => campaign.Organization.Id == id);

            return (query.Skip(offset).Take(limit).ToList(), query.Count());
        }

        public (IList<CampaignModel> campaigns, int count) GetCampaigns(CampaignFiltersDto filters,
            int offset, int limit)
        {
            IQueryable<CampaignModel> query = _context.Campaign.OrderBy(a => a.DateCreation);

            if (filters.OrganizationId != Guid.Empty)
            {
                query = query.Where(campaign => campaign.Organization.Id == filters.OrganizationId);
            }

            query = filters.Status switch
            {
                CampaignStatus.Created => query.Where(campaign => campaign.DateBegin < DateTimeOffset.Now),
                CampaignStatus.Terminated => query.Where(campaign => campaign.DateEnd < DateTimeOffset.Now),
                CampaignStatus.InProgress => query.Where(campaign =>
                    campaign.DateBegin < DateTimeOffset.Now && campaign.DateEnd > DateTimeOffset.Now),
                _ => query
            };

            return (query.Skip(offset).Take(limit).ToList(), query.Count());
        }
    }
}