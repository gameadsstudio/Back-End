using api.Models.Advertisement;
using api.Models.AdContainer;
using api.Models.Campaign;
using api.Models.Tag;
using api.Models.User;
using api.Models.Organization;
using api.Models.Game;
using api.Models.Media;
using api.Models.Media._2D;
using api.Models.Media._3D;
using api.Models.Media.Engine.Unity;
using api.Models.Version;
using api.Models.Post;
using AutoMapper;

namespace api.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User
            CreateMap<UserModel, UserCreationDto>().ReverseMap();
            CreateMap<UserModel, UserUpdateDto>()
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
            CreateMap<UserModel, UserPrivateDto>().ReverseMap();
            CreateMap<UserModel, UserPublicDto>().ReverseMap();

            // Tag
            CreateMap<TagModel, TagCreationDto>().ReverseMap();
            CreateMap<TagModel, TagPublicDto>().ReverseMap();
            CreateMap<TagModel, TagUpdateDto>()
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));

            // Advertisement
            CreateMap<AdvertisementModel, AdvertisementCreationDto>().ReverseMap();
            CreateMap<AdvertisementModel, AdvertisementUpdateDto>()
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
            CreateMap<AdvertisementModel, AdvertisementPublicDto>().ReverseMap();

            // Organization
            CreateMap<OrganizationModel, OrganizationCreationDto>().ReverseMap();
            CreateMap<OrganizationModel, OrganizationUpdateDto>()
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
            CreateMap<OrganizationModel, OrganizationPrivateDto>().ReverseMap();
            CreateMap<OrganizationModel, OrganizationPublicDto>().ReverseMap();

            // AdContainer
            CreateMap<AdContainerModel, AdContainerCreationDto>().ReverseMap();
            CreateMap<AdContainerModel, AdContainerPublicDto>().ReverseMap();
            CreateMap<AdContainerModel, AdContainerUpdateDto>()
                .ReverseMap()
                .ForMember(x => x.Version, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));

            // Game
            CreateMap<GameModel, GameCreationDto>().ReverseMap();
            CreateMap<GameModel, GameUpdateDto>()
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
            CreateMap<GameModel, GamePublicDto>().ReverseMap();

            // Version
            CreateMap<VersionModel, VersionCreationDto>().ReverseMap();
            CreateMap<VersionModel, VersionUpdateDto>()
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
            CreateMap<VersionModel, VersionPublicDto>().ReverseMap();

            // Campaigns
            CreateMap<CampaignModel, CampaignCreationDto>().ReverseMap();
            CreateMap<CampaignModel, CampaignUpdateDto>()
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
            CreateMap<CampaignModel, CampaignPublicDto>().ReverseMap();

            // Medias
            CreateMap<MediaModel, MediaCreationDto>().ReverseMap();
            CreateMap<MediaModel, MediaUpdateDto>().ReverseMap();
            CreateMap<MediaModel, MediaPublicDto>();
            // Medias - 2D
            CreateMap<Media2DModel, Media2DPublicDto>();
            CreateMap<Media2DCreationDto, Media2DModel>().ReverseMap();
            CreateMap<MediaCreationDto, Media2DCreationDto>().ReverseMap();
            // Medias - 3D
            CreateMap<Media3DModel, Media3DPublicDto>();
            CreateMap<Media3DCreationDto, Media3DModel>();
            CreateMap<MediaCreationDto, Media3DCreationDto>();
            // Medias - Unity
            CreateMap<MediaUnityModel, MediaUnityPublicDto>();

            // Blog
            CreateMap<PostModel, PostCreationDto>().ReverseMap();
            CreateMap<PostModel, PostUpdateDto>()
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
            CreateMap<PostModel, PostPublicDto>().ReverseMap();
        }
    }
}
