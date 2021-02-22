using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models.Campaign
{
    [Table("campaign")]
    public class CampaignModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        [Column("org_id")]
        [ForeignKey("OrganizationModel")]
        public Guid OrganizationId { get; set; }
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
    }
}
