﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using api.Contexts;
using api.Models.Campaign;

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
			/* Throw an error :
			Must specify valid information for
			parsing in the string. (Parameter 'value')

            return _context.Campaign
				.Include(x => x.Organization)
				.Where(campaign => campaign.Id == id)
				.SingleOrDefault();
			*/
			return _context.Campaign.SingleOrDefault(
				campaign => campaign.Id == id
			);
        }

        public (IList<CampaignModel>, int) GetOrganizationCampaigns(Guid id, int offset, int limit)
        {
            var query = _context.Campaign.OrderByDescending(
                campaign => campaign.DateCreation
            ).Where(campaign => campaign.Organization.Id == id);

            return (query.Skip(offset).Take(limit).ToList(), query.Count());
        }
    }
}
