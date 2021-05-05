using api.Models.Advertisement;
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
            CreateMap<TagModel, TagCreationModel>().ReverseMap();
            CreateMap<TagModel, TagUpdateModel>().ReverseMap().ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
            
            // Advertisement
            CreateMap<AdvertisementModel, AdvertisementCreationDto>().ReverseMap();
            CreateMap<AdvertisementModel, AdvertisementUpdateDto>().ReverseMap().ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
            CreateMap<AdvertisementModel, AdvertisementPublicDto>().ReverseMap();
            
            // Organization
            CreateMap<OrganizationModel, OrganizationCreationDto>().ReverseMap();
            CreateMap<OrganizationModel, OrganizationUpdateDto>().ReverseMap().ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
            CreateMap<OrganizationModel, OrganizationPrivateDto>().ReverseMap();
            CreateMap<OrganizationModel, OrganizationPublicDto>().ReverseMap();
        }
    }
}