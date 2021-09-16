using System;
using System.Linq;
using api.Enums.Media;
using api.Enums.User;
using api.Enums.Organization;
using api.Models.Advertisement;
using api.Models.AdContainer;
using api.Models.Campaign;
using api.Models.Game;
using api.Models.Media;
using api.Models.Media._2D;
using api.Models.Media._3D;
using api.Models.Media.Engine.Unity;
using api.Models.Organization;
using api.Models.Tag;
using api.Models.User;
using api.Models.Version;
using api.Models.Post;
using Microsoft.EntityFrameworkCore;
using Type = api.Enums.Media.Type;

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
        public DbSet<AdContainerModel> AdContainer { get; set; }
        public DbSet<VersionModel> Version { get; set; }
        public DbSet<MediaModel> Media { get; set; }
        public DbSet<Media2DModel> Media2D { get; set; }
        public DbSet<Media3DModel> Media3D { get; set; }
        public DbSet<MediaUnityModel> MediaUnity { get; set; }
        public DbSet<PostModel> Post { get; set; }

        public override int SaveChanges()
        {
            DateTime saveTime = DateTime.Now;
            foreach (var entry in ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added))
            {
                if (entry.Metadata.FindProperty("DateCreation") == null)
                    continue;
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
                entity.Property(e => e.Role)
                    .HasConversion(
                        v => v.ToString(),
                        v => (UserRole) Enum.Parse(typeof(UserRole), v));
                entity.Property(e => e.Type)
                    .HasConversion(
                        v => v.ToString(),
                        v => (UserType) Enum.Parse(typeof(UserType), v));
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
                entity.Property(e => e.Type)
                    .HasConversion(
                        v => v.ToString(),
                        v => (OrganizationType) Enum.Parse(typeof(OrganizationType), v));
            });

            modelBuilder.Entity<TagModel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<AdvertisementModel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<AdContainerModel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Type)
                    .HasConversion(
                        v => v.ToString(),
                        v => (Type) Enum.Parse(typeof(Type), v));
                entity.Property(e => e.AspectRatio)
                    .HasConversion(
                        v => v.ToString(),
                        v => (AspectRatio) Enum.Parse(typeof(AspectRatio), v));
            });

            modelBuilder.Entity<VersionModel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<MediaModel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Type)
                    .HasConversion(
                        v => v.ToString(),
                        v => (Type) Enum.Parse(typeof(Type), v));
            });

            modelBuilder.Entity<Media2DModel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.AspectRatio)
                    .HasConversion(
                        v => v.ToString(),
                        v => (AspectRatio) Enum.Parse(typeof(AspectRatio), v));
            });

            modelBuilder.Entity<Media3DModel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<MediaUnityModel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<PostModel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });
        }
    }
}
