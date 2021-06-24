using api.Models.Advertisement;
using api.Models.AdContainer;
using api.Models.Tag;
using api.Models.User;
using api.Models.Organization;
using api.Models.Campaign;
using api.Models.Game;
using api.Models.Version;
using AutoMapper;

namespace api.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User
            CreateMap<UserModel, UserCreationDto>().ReverseMap();
            CreateMap<UserModel, UserUpdateDto>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
            CreateMap<UserModel, UserPrivateDto>().ReverseMap();
            CreateMap<UserModel, UserPublicDto>().ReverseMap();

            // Tag
            CreateMap<TagModel, TagCreationDto>().ReverseMap();
            CreateMap<TagModel, TagPublicDto>().ReverseMap();
            CreateMap<TagModel, TagUpdateDto>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));

            // Advertisement
            CreateMap<AdvertisementModel, AdvertisementCreationDto>().ReverseMap();
            CreateMap<AdvertisementModel, AdvertisementUpdateDto>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
            CreateMap<AdvertisementModel, AdvertisementPublicDto>().ReverseMap();

            // Organization
            CreateMap<OrganizationModel, OrganizationCreationDto>().ReverseMap();
            CreateMap<OrganizationModel, OrganizationUpdateDto>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
            CreateMap<OrganizationModel, OrganizationPrivateDto>().ReverseMap();
            CreateMap<OrganizationModel, OrganizationPublicDto>().ReverseMap();

            // AdContainer
            CreateMap<AdContainerModel, AdContainerCreationDto>().ReverseMap();
            CreateMap<AdContainerModel, AdContainerPublicDto>().ReverseMap();
            CreateMap<AdContainerModel, AdContainerUpdateDto>().ReverseMap()
                .ForMember(x => x.Version, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));

            // Campaign
            CreateMap<CampaignModel, CampaignCreationDto>().ReverseMap();
            CreateMap<CampaignModel, CampaignPublicDto>().ReverseMap();
            CreateMap<CampaignModel, CampaignUpdateDto>().ReverseMap();

            // Game
            CreateMap<GameModel, GameCreationDto>().ReverseMap();
            CreateMap<GameModel, GameUpdateDto>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
            CreateMap<GameModel, GamePublicDto>().ReverseMap();

            // Version
            CreateMap<VersionModel, VersionCreationDto>().ReverseMap();
            CreateMap<VersionModel, VersionUpdateDto>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
            CreateMap<VersionModel, VersionPublicDto>().ReverseMap();
        }
    }
}
