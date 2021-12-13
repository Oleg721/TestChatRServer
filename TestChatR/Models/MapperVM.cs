using AutoMapper;
using CoreLib.Requests;
using DAL.Models;
using DTO;
using DTO.Authentications;
using Models.Requests;
using Models.Responses;
using TestChatR.Models;

namespace TestChatR.Models
{
    class MapperVM: Profile
    {
        public MapperVM()
        {
            MapConfig(this);
        }

        private static void MapConfig(IProfileExpression cfg)
        {
            cfg.CreateMap<UserVM, UserDto>().ReverseMap();
            cfg.CreateMap<MessageVM, MessageDto>().ReverseMap();
            cfg.CreateMap<RegisterRequest, RegisterDto>().ReverseMap();
            cfg.CreateMap<User, UserVM>().ReverseMap();
            cfg.CreateMap<LoginRequest, LoginDto>().ReverseMap();
            cfg.CreateMap<AuthenticatedUserDto, AuthenticatedUserResponse>().ReverseMap();
        }
    }
}
