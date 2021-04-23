using api.Models.User;
using AutoMapper;

namespace api.Mappings
{
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<UserModel, UserCreationModel>().ReverseMap();
            CreateMap<UserModel, UserUpdateModel>().ReverseMap();
        }
    }
}