
using AutoMapper;
using DAL.Models;
using DTO;
using DTO.Authentications;

namespace DAL
{
    public class MapperDAL : Profile
    {
        public MapperDAL()
        {
            MapConfig(this);
        }
        private static void MapConfig(IProfileExpression cfg)
        {
            cfg.CreateMap<User, UserDto>().ReverseMap();
            cfg.CreateMap<Message, MessageDto>().ReverseMap();
            cfg.CreateMap<User, RegisterDto>().ReverseMap();
        }
    }
}
