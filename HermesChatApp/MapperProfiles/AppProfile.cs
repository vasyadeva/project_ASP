using AutoMapper;
using DataLayer;
using LinkedNewsChatApp.Models;

namespace LinkedNewsChatApp.MapperProfiles
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<User, UserModel>()
                .ReverseMap();

            CreateMap<Group, GroupModel>()
                .ReverseMap();
        }
    }
}
