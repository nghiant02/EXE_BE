using AutoMapper;
using EXE201.BLL.DTOs.UserDTOs;
using EXE201.DAL.DTOs.UserDTOs;
using EXE201.DAL.Models;

namespace EXE201.BLL.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            UserMappingProfile();
        }

        private void UserMappingProfile()
        {
            CreateMap<User, GetUserDTOs>();
            CreateMap<RegisterUserDTOs, User>();
        }
    }
}
