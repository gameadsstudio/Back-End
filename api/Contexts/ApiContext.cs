using api.Models.Campaign;
using api.Models.Game;
using api.Models.Organization;
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

        public virtual DbSet<UserModel> User { get; set; }
        public virtual DbSet<CampaignModel> Campaign { get; set; }
        public virtual DbSet<GameModel> Game { get; set; }
        public virtual DbSet<OrganizationModel> Organization { get; set; }

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
        }
    }
}
