using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api.Enums.Campaign;
using api.Models.Advertisement;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using api.Models.Organization;
using api.Models.Tag;

namespace api.Models.Campaign
{
    [Table("campaign")]
    public class CampaignModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int AgeMin { get; set; }

        public int AgeMax { get; set; }

        public DateTimeOffset DateBegin { get; set; }
        
        public DateTimeOffset DateEnd { get; set; }
        
        public DateTimeOffset DateCreation { get; set; }

        public DateTimeOffset DateUpdate { get; set; }

        [BindRequired]
        [ForeignKey("OrgId")]
        public OrganizationModel Organization { get; set; }
        
        public IList<AdvertisementModel> Advertisements { get; set; }
    }
}