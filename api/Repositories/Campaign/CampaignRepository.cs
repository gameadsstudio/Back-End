﻿using System;
using System.Collections.Generic;
using System.Linq;
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

		public List<CampaignModel> GetOrganizationCampaigns(Guid id)
		{
			return _context.Campaign
				.Where(x => x.Organization.Id == id)
				.ToList();
		}
    }
}
