﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using api.Contexts;

namespace api.Migrations
{
    [DbContext(typeof(ApiContext))]
    partial class ApiContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("api.Models.Advertisement.AdvertisementModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("AgeMax")
                        .HasColumnType("integer")
                        .HasColumnName("age_max");

                    b.Property<int>("AgeMin")
                        .HasColumnType("integer")
                        .HasColumnName("age_min");

                    b.Property<DateTimeOffset>("DateCreation")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_creation");

                    b.Property<DateTimeOffset>("DateUpdate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_update");

                    b.Property<string>("Status")
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.HasKey("Id")
                        .HasName("pk_advertisement");

                    b.ToTable("advertisement");
                });

            modelBuilder.Entity("api.Models.Campaign.CampaignModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("AgeMax")
                        .HasColumnType("text")
                        .HasColumnName("age_max");

                    b.Property<string>("AgeMin")
                        .HasColumnType("text")
                        .HasColumnName("age_min");

                    b.Property<DateTimeOffset>("DateBegin")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_begin");

                    b.Property<DateTimeOffset>("DateCreation")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_creation");

                    b.Property<DateTimeOffset>("DateEnd")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_end");

                    b.Property<DateTimeOffset>("DateUpdate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_update");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Guid>("OrgId")
                        .HasColumnType("uuid")
                        .HasColumnName("org_id");

                    b.Property<string>("Status")
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.Property<string>("Type")
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.HasKey("Id")
                        .HasName("pk_campaign");

                    b.HasIndex("OrgId")
                        .HasDatabaseName("ix_campaign_org_id");

                    b.ToTable("campaign");
                });

            modelBuilder.Entity("api.Models.Game.GameModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("DateCreation")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_creation");

                    b.Property<DateTimeOffset>("DateLaunch")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_launch");

                    b.Property<DateTimeOffset>("DateUpdate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_update");

                    b.Property<Guid>("MediaId")
                        .HasColumnType("uuid")
                        .HasColumnName("media_id");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Guid>("OrgId")
                        .HasColumnType("uuid")
                        .HasColumnName("org_id");

                    b.Property<string>("Status")
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.HasKey("Id")
                        .HasName("pk_game");

                    b.HasIndex("OrgId")
                        .HasDatabaseName("ix_game_org_id");

                    b.ToTable("game");
                });

            modelBuilder.Entity("api.Models.Organization.OrganizationModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreationDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("creation_date");

                    b.Property<string>("DefaultAuthorization")
                        .HasColumnType("text")
                        .HasColumnName("default_authorization");

                    b.Property<string>("Localization")
                        .HasColumnType("text")
                        .HasColumnName("localization");

                    b.Property<string>("LogoUrl")
                        .HasColumnType("text")
                        .HasColumnName("logo_url");

                    b.Property<DateTimeOffset>("ModificationDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("modification_date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("PrivateEmail")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("private_email");

                    b.Property<string>("PublicEmail")
                        .HasColumnType("text")
                        .HasColumnName("public_email");

                    b.Property<string>("State")
                        .HasColumnType("text")
                        .HasColumnName("state");

                    b.Property<string>("Type")
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.Property<string>("WebsiteUrl")
                        .HasColumnType("text")
                        .HasColumnName("website_url");

                    b.HasKey("Id")
                        .HasName("pk_organization");

                    b.ToTable("organization");
                });

            modelBuilder.Entity("api.Models.Tag.TagModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("DateCreation")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_creation");

                    b.Property<DateTimeOffset>("DateUpdate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_update");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_tag");

                    b.ToTable("tag");
                });

            modelBuilder.Entity("api.Models.User.UserModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Alias")
                        .HasColumnType("text")
                        .HasColumnName("alias");

                    b.Property<DateTimeOffset>("DateCreation")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_creation");

                    b.Property<string>("DateStatus")
                        .HasColumnType("text")
                        .HasColumnName("date_status");

                    b.Property<DateTimeOffset>("DateUpdate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_update");

                    b.Property<string>("Email")
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<string>("Level")
                        .HasColumnType("text")
                        .HasColumnName("level");

                    b.Property<string>("Password")
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<string>("Phone")
                        .HasColumnType("text")
                        .HasColumnName("phone");

                    b.Property<string>("Status")
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.Property<string>("Username")
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasKey("Id")
                        .HasName("pk_user");

                    b.ToTable("user");
                });

            modelBuilder.Entity("api.Models.Campaign.CampaignModel", b =>
                {
                    b.HasOne("api.Models.Organization.OrganizationModel", "Organization")
                        .WithMany("Campaigns")
                        .HasForeignKey("OrgId")
                        .HasConstraintName("fk_campaign_organization_org_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("api.Models.Game.GameModel", b =>
                {
                    b.HasOne("api.Models.Organization.OrganizationModel", "Organization")
                        .WithMany("Games")
                        .HasForeignKey("OrgId")
                        .HasConstraintName("fk_game_organization_org_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("api.Models.Organization.OrganizationModel", b =>
                {
                    b.Navigation("Campaigns");

                    b.Navigation("Games");
                });
#pragma warning restore 612, 618
        }
    }
}
