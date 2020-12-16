using Microsoft.EntityFrameworkCore;

using GAME_ADS_STUDIO_API.Models.Organization;
using GAME_ADS_STUDIO_API.Models.User;
using GAME_ADS_STUDIO_API.Models.Campaign;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAME_ADS_STUDIO_API.Contexts
{
    public class GasContext:DbContext
    {
        public GasContext()
        {
        }

        public GasContext(DbContextOptions<GasContext>options):base(options)
        {
        }
        public virtual DbSet<UserModel> User { get; set; }
        public virtual DbSet<OrganizationModel> Organization { get; set; }
        public virtual DbSet<CampaignModel> Campaign { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrganizationModel>(entity =>
            {
                entity.ToTable("organizations", "gas_dev");

                entity.HasKey(e => e.org_id);

                entity.Property(e => e.org_id)
                .HasColumnName("org_id");

                entity.Property(e => e.media_id)
                .HasColumnName("media_id");

                entity.Property(e => e.org_name)
                .IsRequired()
                .HasColumnName("org_name")
                .HasMaxLength(128);

                entity.Property(e => e.org_email)
                .IsRequired()
                .HasColumnName("org_email")
                .HasMaxLength(320);

                entity.Property(e => e.org_email_private)
                .HasColumnName("org_email_private")
                .HasMaxLength(320);

                entity.Property(e => e.org_city)
                .HasColumnName("org_city")
                .HasMaxLength(85);

                entity.Property(e => e.org_address)
                .HasColumnName("org_address")
                .HasMaxLength(95);

                entity.Property(e => e.org_url)
                .HasColumnName("org_url")
                .HasMaxLength(2048);

                entity.Property(e => e.org_type)
                .IsRequired()
                .HasColumnName("org_type")
                .HasMaxLength(1);

                entity.Property(e => e.org_status)
                .HasColumnName("org_status")
                .HasMaxLength(1);

                entity.Property(e => e.org_level_default)
                .HasColumnName("org_level_default")
                .HasColumnType("int");

                entity.Property(e => e.org_date_status)
                .HasColumnName("org_date_status");

                entity.Property(e => e.org_date_creation)
                .HasColumnName("org_date_creation")
                .HasDefaultValueSql("current_timestamp()"); ;

                entity.Property(e => e.org_date_update)
                .HasColumnName("org_date_update");
            });

            modelBuilder.Entity<CampaignModel>(entity =>
            {
                entity.ToTable("users", "gas_dev");

                entity.HasKey(e => e.cpg_id);

                entity.Property(e => e.cpg_id)
                .HasColumnName("cpg_id");

                entity.Property(e => e.org_id)
                .IsRequired()
                .HasColumnName("org_id");

                entity.Property(e => e.cpg_name)
                .IsRequired()
                .HasColumnName("cpg_name")
                .HasMaxLength(128);

                entity.Property(e => e.cpg_age_min)
                .HasColumnName("cpg_age_min")
                .HasColumnType("int");

                entity.Property(e => e.cpg_age_max)
                .HasColumnName("cpg_age_max")
                .HasColumnType("int");

                entity.Property(e => e.cpg_date_begin)
                .HasColumnName("cpg_date_begin");

                entity.Property(e => e.cpg_date_end)
                .HasColumnName("cpg_date_end");

                entity.Property(e => e.cpg_date_creation)
                .HasColumnName("cpg_date_creation")
                .HasDefaultValueSql("current_timestamp()"); ;

                entity.Property(e => e.cpg_date_update)
                .HasColumnName("cpg_date_update");
            });
        }
    }
}
