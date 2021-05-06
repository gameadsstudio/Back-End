using api.Models.Advertisement;
using api.Models.AdContainer;
using api.Models.Tag;
using api.Models.User;
using api.Models.Organization;
using AutoMapper;

namespace api.Mappings
{
    public class MappingProfile : Profile {
        public MappingProfile() {
            // User
            CreateMap<UserModel, UserCreationDto>().ReverseMap();
            CreateMap<UserModel, UserUpdateDto>().ReverseMap().ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
            CreateMap<UserModel, UserPrivateDto>().ReverseMap();
            CreateMap<UserModel, UserPublicDto>().ReverseMap();

            // Tag
            CreateMap<TagModel, TagCreationDto>().ReverseMap();
            CreateMap<TagModel, TagPublicDto>().ReverseMap();
            CreateMap<TagModel, TagUpdateDto>().ReverseMap().ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));

            // Advertisement
            CreateMap<AdvertisementModel, AdvertisementCreationDto>().ReverseMap();
            CreateMap<AdvertisementModel, AdvertisementUpdateDto>().ReverseMap().ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
            CreateMap<AdvertisementModel, AdvertisementPublicDto>().ReverseMap();

            // Organization
            CreateMap<OrganizationModel, OrganizationCreationDto>().ReverseMap();
            CreateMap<OrganizationModel, OrganizationUpdateDto>().ReverseMap().ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
            CreateMap<OrganizationModel, OrganizationPrivateDto>().ReverseMap();
            CreateMap<OrganizationModel, OrganizationPublicDto>().ReverseMap();

            // AdContainer
            CreateMap<AdContainerModel, AdContainerCreationDto>().ReverseMap();
            CreateMap<AdContainerModel, AdContainerPublicDto>().ReverseMap();
            CreateMap<AdContainerModel, AdContainerUpdateDto>().ReverseMap().ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
        }
    }
}