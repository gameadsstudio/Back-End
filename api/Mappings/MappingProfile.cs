using api.Models.Advertisement;
using api.Models.Tag;
using api.Models.User;
using AutoMapper;

namespace api.Mappings
{
    public class MappingProfile : Profile {
        public MappingProfile() {
            // User
            CreateMap<UserModel, UserCreationModel>().ReverseMap();
            CreateMap<UserModel, UserUpdateModel>().ReverseMap().ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
            CreateMap<UserModel, UserPrivateModel>().ReverseMap();
            CreateMap<UserModel, UserPublicModel>().ReverseMap();

            // Tag
            CreateMap<TagModel, TagCreationModel>().ReverseMap();
            CreateMap<TagModel, TagUpdateModel>().ReverseMap().ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
            
            // Advertisement
            CreateMap<AdvertisementModel, AdvertisementCreationModel>().ReverseMap();
            CreateMap<AdvertisementModel, AdvertisementUpdateModel>().ReverseMap().ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));;
        }
    }
}