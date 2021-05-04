using api.Models.Tag;
using api.Models.User;
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
        }
    }
}