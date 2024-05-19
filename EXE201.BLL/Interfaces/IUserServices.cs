using EXE201.BLL.DTOs.UserDTOs;
using EXE201.DAL.DTOs.UserDTOs;
using EXE201.DAL.Models;
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
    }
}
