using EXE201.BLL.DTOs.UserDTOs;
using EXE201.DAL.DTOs;
using EXE201.DAL.DTOs.UserDTOs;
using EXE201.DAL.Models;
using LMSystem.Repository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.BLL.Interfaces
{
    public interface IUserServices
    {
        Task<GetUserDTOs> Login(string username, string password);
        Task<IEnumerable<User>> GetAllProfileUser();
        Task<bool> ChangeStatusUserToNotActive(int userId);
        Task<User> AddUserForStaff(AddNewUserDTO addNewUserDTO);
        Task<GetUserDTOs> Register(RegisterUserDTOs registerUserDTOs);
        Task<User> ChangePasword(int id, ChangePasswordDTO changePasswordDTO);
        Task<User> FindUserByEmail(string email);
        Task<User> UpdatePassword(string email, string password, int id);
        Task<User> UserUpdateUser(int id, UpdateProfileUserDTO userView);
        Task<PagedResponseDTO<UserListDTO>> GetFilteredUser(UserFilterDTO filter);
        Task<UserProfileDTO> GetUserProfile(int userId);
    }
}
