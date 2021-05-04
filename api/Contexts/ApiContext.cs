using System;
using System.Linq;
using api.Enums;
using api.Models.Advertisement;
using api.Models.Campaign;
using api.Models.Game;
using api.Models.Organization;
using api.Models.Tag;
using api.Models.User;
using Microsoft.EntityFrameworkCore;

namespace api.Contexts
{
    public class ApiContext: DbContext
    {
        public ApiContext() {}

        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
        }

        public DbSet<UserModel> User { get; set; }
        public DbSet<CampaignModel> Campaign { get; set; }
        public DbSet<GameModel> Game { get; set; }
        public DbSet<OrganizationModel> Organization { get; set; }
        public DbSet<TagModel> Tag { get; set; }
        public DbSet<AdvertisementModel> Advertisement { get; set; }

        public override int SaveChanges()
        {
            DateTime saveTime = DateTime.Now;
            foreach (var entry in ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added))
            {
                if (entry.Property("DateCreation").CurrentValue == null || (DateTimeOffset)entry.Property("DateCreation").CurrentValue == DateTime.MinValue)
                {
                    entry.Property("DateCreation").CurrentValue = (DateTimeOffset)saveTime;
                }
                entry.Property("DateUpdate").CurrentValue = (DateTimeOffset)saveTime;
            }
            foreach (var entry in ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified))
            {
                entry.Property("DateUpdate").CurrentValue = (DateTimeOffset)saveTime;
            }
            return base.SaveChanges();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<CampaignModel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<GameModel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<OrganizationModel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<TagModel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });
            
            modelBuilder.Entity<AdvertisementModel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Status)
                    .HasConversion(
                        v => v.ToString(),
                        v => (AdvertisementStatus) Enum.Parse(typeof(AdvertisementStatus), v));
            });
        }
    }
}
