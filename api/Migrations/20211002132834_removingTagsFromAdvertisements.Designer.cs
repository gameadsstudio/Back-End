﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using api.Contexts;

namespace api.Migrations
{
    [DbContext(typeof(ApiContext))]
    [Migration("20211002132834_removingTagsFromAdvertisements")]
    partial class removingTagsFromAdvertisements
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("AdContainerModelTagModel", b =>
                {
                    b.Property<Guid>("AdContainersId")
                        .HasColumnType("uuid")
                        .HasColumnName("ad_containers_id");

                    b.Property<Guid>("TagsId")
                        .HasColumnType("uuid")
                        .HasColumnName("tags_id");

                    b.HasKey("AdContainersId", "TagsId")
                        .HasName("pk_ad_container_model_tag_model");

                    b.HasIndex("TagsId")
                        .HasDatabaseName("ix_ad_container_model_tag_model_tags_id");

                    b.ToTable("ad_container_model_tag_model");
                });

            modelBuilder.Entity("MediaModelTagModel", b =>
                {
                    b.Property<Guid>("MediasId")
                        .HasColumnType("uuid")
                        .HasColumnName("medias_id");

                    b.Property<Guid>("TagsId")
                        .HasColumnType("uuid")
                        .HasColumnName("tags_id");

                    b.HasKey("MediasId", "TagsId")
                        .HasName("pk_media_model_tag_model");

                    b.HasIndex("TagsId")
                        .HasDatabaseName("ix_media_model_tag_model_tags_id");

                    b.ToTable("media_model_tag_model");
                });

            modelBuilder.Entity("OrganizationModelUserModel", b =>
                {
                    b.Property<Guid>("OrganizationsId")
                        .HasColumnType("uuid")
                        .HasColumnName("organizations_id");

                    b.Property<Guid>("UsersId")
                        .HasColumnType("uuid")
                        .HasColumnName("users_id");

                    b.HasKey("OrganizationsId", "UsersId")
                        .HasName("pk_organization_model_user_model");

                    b.HasIndex("UsersId")
                        .HasDatabaseName("ix_organization_model_user_model_users_id");

                    b.ToTable("organization_model_user_model");
                });

            modelBuilder.Entity("api.Models.AdContainer.AdContainerModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("AspectRatio")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("aspect_ratio");

                    b.Property<DateTimeOffset>("DateCreation")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_creation");

                    b.Property<DateTimeOffset>("DateUpdate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_update");

                    b.Property<int>("Depth")
                        .HasColumnType("integer")
                        .HasColumnName("depth");

                    b.Property<int>("Height")
                        .HasColumnType("integer")
                        .HasColumnName("height");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Guid?>("OrgId")
                        .HasColumnType("uuid")
                        .HasColumnName("org_id");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.Property<Guid?>("VersionId")
                        .HasColumnType("uuid")
                        .HasColumnName("version_id");

                    b.Property<int>("Width")
                        .HasColumnType("integer")
                        .HasColumnName("width");

                    b.HasKey("Id")
                        .HasName("pk_ad_container");

                    b.HasIndex("OrgId")
                        .HasDatabaseName("ix_ad_container_org_id");

                    b.HasIndex("VersionId")
                        .HasDatabaseName("ix_ad_container_version_id");

                    b.ToTable("ad_container");
                });

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

                    b.Property<Guid?>("CampaignId")
                        .HasColumnType("uuid")
                        .HasColumnName("campaign_id");

                    b.Property<DateTimeOffset>("DateCreation")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_creation");

                    b.Property<DateTimeOffset>("DateUpdate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_update");

                    b.Property<Guid?>("MediaId")
                        .HasColumnType("uuid")
                        .HasColumnName("media_id");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Guid?>("TagModelId")
                        .HasColumnType("uuid")
                        .HasColumnName("tag_model_id");

                    b.HasKey("Id")
                        .HasName("pk_advertisement");

                    b.HasIndex("CampaignId")
                        .HasDatabaseName("ix_advertisement_campaign_id");

                    b.HasIndex("MediaId")
                        .HasDatabaseName("ix_advertisement_media_id");

                    b.HasIndex("TagModelId")
                        .HasDatabaseName("ix_advertisement_tag_model_id");

                    b.ToTable("advertisement");
                });

            modelBuilder.Entity("api.Models.Campaign.CampaignModel", b =>
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

                    b.Property<Guid?>("OrgId")
                        .HasColumnType("uuid")
                        .HasColumnName("org_id");

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

                    b.Property<Guid?>("OrgId")
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

            modelBuilder.Entity("api.Models.Media.Engine.Unity.MediaUnityModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("AssetBundleLink")
                        .HasColumnType("text")
                        .HasColumnName("asset_bundle_link");

                    b.Property<DateTimeOffset>("DateCreation")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_creation");

                    b.Property<DateTimeOffset>("DateUpdate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_update");

                    b.Property<Guid>("MediaId")
                        .HasColumnType("uuid")
                        .HasColumnName("media_id");

                    b.Property<int>("State")
                        .HasColumnType("integer")
                        .HasColumnName("state");

                    b.Property<string>("StateMessage")
                        .HasColumnType("text")
                        .HasColumnName("state_message");

                    b.HasKey("Id")
                        .HasName("pk_media_unity");

                    b.HasIndex("MediaId")
                        .HasDatabaseName("ix_media_unity_media_id");

                    b.ToTable("media_unity");
                });

            modelBuilder.Entity("api.Models.Media.MediaModel", b =>
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

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Guid?>("OrgId")
                        .HasColumnType("uuid")
                        .HasColumnName("org_id");

                    b.Property<int>("State")
                        .HasColumnType("integer")
                        .HasColumnName("state");

                    b.Property<string>("StateMessage")
                        .HasColumnType("text")
                        .HasColumnName("state_message");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.HasKey("Id")
                        .HasName("pk_media");

                    b.HasIndex("OrgId")
                        .HasDatabaseName("ix_media_org_id");

                    b.ToTable("media");
                });

            modelBuilder.Entity("api.Models.Media._2D.Media2DModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("AspectRatio")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("aspect_ratio");

                    b.Property<DateTimeOffset>("DateCreation")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_creation");

                    b.Property<DateTimeOffset>("DateUpdate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_update");

                    b.Property<Guid>("MediaId")
                        .HasColumnType("uuid")
                        .HasColumnName("media_id");

                    b.Property<string>("NormalMapLink")
                        .HasColumnType("text")
                        .HasColumnName("normal_map_link");

                    b.Property<string>("TextureLink")
                        .HasColumnType("text")
                        .HasColumnName("texture_link");

                    b.HasKey("Id")
                        .HasName("pk_media_2d");

                    b.HasIndex("MediaId")
                        .HasDatabaseName("ix_media_2d_media_id");

                    b.ToTable("media_2D");
                });

            modelBuilder.Entity("api.Models.Media._3D.Media3DModel", b =>
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

                    b.Property<int>("Depth")
                        .HasColumnType("integer")
                        .HasColumnName("depth");

                    b.Property<int>("Height")
                        .HasColumnType("integer")
                        .HasColumnName("height");

                    b.Property<Guid>("MediaId")
                        .HasColumnType("uuid")
                        .HasColumnName("media_id");

                    b.Property<string>("ModelLink")
                        .HasColumnType("text")
                        .HasColumnName("model_link");

                    b.Property<string>("NormalMapLink")
                        .HasColumnType("text")
                        .HasColumnName("normal_map_link");

                    b.Property<string>("TextureLink")
                        .HasColumnType("text")
                        .HasColumnName("texture_link");

                    b.Property<int>("Width")
                        .HasColumnType("integer")
                        .HasColumnName("width");

                    b.HasKey("Id")
                        .HasName("pk_media_3d");

                    b.HasIndex("MediaId")
                        .HasDatabaseName("ix_media_3d_media_id");

                    b.ToTable("media_3D");
                });

            modelBuilder.Entity("api.Models.Organization.OrganizationModel", b =>
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

                    b.Property<string>("Email")
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("LogoUrl")
                        .HasColumnType("text")
                        .HasColumnName("logo_url");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Type")
                        .IsRequired()
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

                    b.Property<DateTimeOffset>("DateCreation")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_creation");

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

                    b.Property<string>("Password")
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("role");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.Property<string>("Username")
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasKey("Id")
                        .HasName("pk_user");

                    b.ToTable("user");
                });

            modelBuilder.Entity("api.Models.Version.VersionModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid?>("GameId")
                        .HasColumnType("uuid")
                        .HasColumnName("game_id");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("SemVer")
                        .HasColumnType("text")
                        .HasColumnName("sem_ver");

                    b.HasKey("Id")
                        .HasName("pk_version");

                    b.HasIndex("GameId")
                        .HasDatabaseName("ix_version_game_id");

                    b.ToTable("version");
                });

            modelBuilder.Entity("AdContainerModelTagModel", b =>
                {
                    b.HasOne("api.Models.AdContainer.AdContainerModel", null)
                        .WithMany()
                        .HasForeignKey("AdContainersId")
                        .HasConstraintName("fk_ad_container_model_tag_model_ad_container_ad_containers_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api.Models.Tag.TagModel", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .HasConstraintName("fk_ad_container_model_tag_model_tag_tags_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MediaModelTagModel", b =>
                {
                    b.HasOne("api.Models.Media.MediaModel", null)
                        .WithMany()
                        .HasForeignKey("MediasId")
                        .HasConstraintName("fk_media_model_tag_model_media_medias_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api.Models.Tag.TagModel", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .HasConstraintName("fk_media_model_tag_model_tag_tags_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OrganizationModelUserModel", b =>
                {
                    b.HasOne("api.Models.Organization.OrganizationModel", null)
                        .WithMany()
                        .HasForeignKey("OrganizationsId")
                        .HasConstraintName("fk_organization_model_user_model_organization_organizations_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api.Models.User.UserModel", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .HasConstraintName("fk_organization_model_user_model_user_users_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("api.Models.AdContainer.AdContainerModel", b =>
                {
                    b.HasOne("api.Models.Organization.OrganizationModel", "Organization")
                        .WithMany()
                        .HasForeignKey("OrgId")
                        .HasConstraintName("fk_ad_container_organization_org_id");

                    b.HasOne("api.Models.Version.VersionModel", "Version")
                        .WithMany()
                        .HasForeignKey("VersionId")
                        .HasConstraintName("fk_ad_container_version_version_id");

                    b.Navigation("Organization");

                    b.Navigation("Version");
                });

            modelBuilder.Entity("api.Models.Advertisement.AdvertisementModel", b =>
                {
                    b.HasOne("api.Models.Campaign.CampaignModel", "Campaign")
                        .WithMany("Advertisements")
                        .HasForeignKey("CampaignId")
                        .HasConstraintName("fk_advertisement_campaign_campaign_id");

                    b.HasOne("api.Models.Media.MediaModel", "Media")
                        .WithMany()
                        .HasForeignKey("MediaId")
                        .HasConstraintName("fk_advertisement_media_media_id");

                    b.HasOne("api.Models.Tag.TagModel", null)
                        .WithMany("Advertisements")
                        .HasForeignKey("TagModelId")
                        .HasConstraintName("fk_advertisement_tag_tag_model_id");

                    b.Navigation("Campaign");

                    b.Navigation("Media");
                });

            modelBuilder.Entity("api.Models.Campaign.CampaignModel", b =>
                {
                    b.HasOne("api.Models.Organization.OrganizationModel", "Organization")
                        .WithMany("Campaigns")
                        .HasForeignKey("OrgId")
                        .HasConstraintName("fk_campaign_organization_org_id");

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("api.Models.Game.GameModel", b =>
                {
                    b.HasOne("api.Models.Organization.OrganizationModel", "Organization")
                        .WithMany("Games")
                        .HasForeignKey("OrgId")
                        .HasConstraintName("fk_game_organization_org_id");

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("api.Models.Media.Engine.Unity.MediaUnityModel", b =>
                {
                    b.HasOne("api.Models.Media.MediaModel", "Media")
                        .WithMany()
                        .HasForeignKey("MediaId")
                        .HasConstraintName("fk_media_unity_media_media_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Media");
                });

            modelBuilder.Entity("api.Models.Media.MediaModel", b =>
                {
                    b.HasOne("api.Models.Organization.OrganizationModel", "Organization")
                        .WithMany()
                        .HasForeignKey("OrgId")
                        .HasConstraintName("fk_media_organization_org_id");

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("api.Models.Media._2D.Media2DModel", b =>
                {
                    b.HasOne("api.Models.Media.MediaModel", "Media")
                        .WithMany()
                        .HasForeignKey("MediaId")
                        .HasConstraintName("fk_media_2d_media_media_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Media");
                });

            modelBuilder.Entity("api.Models.Media._3D.Media3DModel", b =>
                {
                    b.HasOne("api.Models.Media.MediaModel", "Media")
                        .WithMany()
                        .HasForeignKey("MediaId")
                        .HasConstraintName("fk_media_3d_media_media_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Media");
                });

            modelBuilder.Entity("api.Models.Version.VersionModel", b =>
                {
                    b.HasOne("api.Models.Game.GameModel", "Game")
                        .WithMany("Versions")
                        .HasForeignKey("GameId")
                        .HasConstraintName("fk_version_game_game_id");

                    b.Navigation("Game");
                });

            modelBuilder.Entity("api.Models.Campaign.CampaignModel", b =>
                {
                    b.Navigation("Advertisements");
                });

            modelBuilder.Entity("api.Models.Game.GameModel", b =>
                {
                    b.Navigation("Versions");
                });

            modelBuilder.Entity("api.Models.Organization.OrganizationModel", b =>
                {
                    b.Navigation("Campaigns");

                    b.Navigation("Games");
                });

            modelBuilder.Entity("api.Models.Tag.TagModel", b =>
                {
                    b.Navigation("Advertisements");
                });
#pragma warning restore 612, 618
        }
    }
}
