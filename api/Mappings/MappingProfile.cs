using api.Models.User;
using AutoMapper;

namespace api.Mappings
{
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<UserModel, UserCreationModel>().ReverseMap();
            CreateMap<UserModel, UserUpdateModel>().ReverseMap().ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
            CreateMap<UserModel, UserPrivateModel>().ReverseMap();
            CreateMap<UserModel, UserPublicModel>().ReverseMap();
        }
    }
}