using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GAME_ADS_STUDIO_API.Configuration;
using GAME_ADS_STUDIO_API.Contexts;
using Microsoft.Extensions.Options;
using GAME_ADS_STUDIO_API.Models.Campaign;
using GAME_ADS_STUDIO_API.Repositories.Campaign;


namespace GAME_ADS_STUDIO_API.Business.Campaign
{
    public class CampaignBusinessLogic : ICampaignBusinessLogic
    {
        private readonly ICampaignRepository _repository;
        private readonly AppSettings _appSettings;

        public CampaignBusinessLogic(GasContext context, IOptions<AppSettings> appSettings)
        {
            _repository = new CampaignRepository(context);
            _appSettings = appSettings.Value;
        }

        public CampaignModel AddNewCampaign(CampaignCreationModel newCampaign)
        {

            var Campaign = new CampaignModel {
                org_id = newCampaign.org_id,
                cpg_name = newCampaign.cpg_name,
                cpg_type = newCampaign.cpg_type
            };

            return _repository.AddNewCampaign(Campaign) == 1 ? Campaign : null;
        }

        public CampaignPublicModel[] GetCampaigns(int offset, int limit)
        {
            return _repository.GetCampaigns(offset, limit);
        }

        public CampaignModel GetCampaignById(int id)
        {
            if (id < 0)
                return null;

            var Campaign = _repository.GetCampaignById(id);

            return Campaign;
        }

        public int UpdateCampaignById(int id, CampaignUpdateModel updatedCampaign)
        {
            var target = _repository.GetCampaignById(id);

            if (target == null)
                return 2;
            if (updatedCampaign == null)
                return 0;

            if (updatedCampaign.org_id == null)
                updatedCampaign.org_id = target.org_id;

            if (updatedCampaign.cpg_name == null)
                updatedCampaign.cpg_name = target.cpg_name;

            if (updatedCampaign.cpg_age_min == null)
                updatedCampaign.cpg_age_min = target.cpg_age_min;

            if (updatedCampaign.cpg_age_max == null)
                updatedCampaign.cpg_age_max = target.cpg_age_max;

            if (updatedCampaign.cpg_type == null)
                updatedCampaign.cpg_type = target.cpg_type;

            if (updatedCampaign.cpg_status == null)
                updatedCampaign.cpg_status = target.cpg_status;

            if (updatedCampaign.cpg_date_begin == null)
                updatedCampaign.cpg_date_begin = target.cpg_date_begin;

            if (updatedCampaign.cpg_date_end == null)
                updatedCampaign.cpg_date_end = target.cpg_date_end;

            return _repository.UpdateCampaign(updatedCampaign, target);
        }

        public int DeleteCampaignById(int id)
        {
            if (id < 0)
                return 0;

            var Campaign = _repository.GetCampaignById(id);

            if (Campaign == null)
                return 0;

            return _repository.DeleteCampaign(Campaign);
        }
    }
}
