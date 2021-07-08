using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using api.Models.Organization;

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

        public string AgeMin { get; set; }

        public string AgeMax { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }

        public DateTimeOffset DateBegin { get; set; }

        public DateTimeOffset DateEnd { get; set; }

        public DateTimeOffset DateCreation { get; set; }

        public DateTimeOffset DateUpdate { get; set; }

        [BindRequired]
        [ForeignKey("OrgId")]
        public OrganizationModel Organization { get; set; }
    }
}
