// EXE201.BLL.Services.UserServices.cs
using AutoMapper;
using EXE201.BLL.DTOs.UserDTOs;
using EXE201.BLL.Interfaces;
using EXE201.DAL.DTOs.UserDTOs;
using EXE201.DAL.Interfaces;
using EXE201.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.BLL.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;

        public UserServices(IUserRepository userRepository, IMapper mapper, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _roleRepository = roleRepository;
        }

        public async Task<User> AddUserForStaff(AddNewUserDTO addNewUserDTO)
        {
            var mapUser = _mapper.Map<User>(addNewUserDTO);

            await _userRepository.AddNewUser(mapUser);
            return mapUser;
        }

        public async Task<bool> ChangeStatusUserToNotActive(int userId)
        {
            var existUser = await _userRepository.GetUserById(userId);
            if (existUser == null)
            {
                throw new ArgumentException("Id does not exist!!");
            }
            await _userRepository.ChangeStatusUserToNotActive(existUser.UserID);
            return true;
        }

        public async Task<IEnumerable<User>> GetAllProfileUser()
        {
            var allUser = await _userRepository.GetAllUsers();
            if (allUser == null)
            {
                throw new ArgumentException("Do not exist User");
            }
            return allUser;
        }

        public async Task<GetUserDTOs> Login(string username, string password)
        {
            var user = await _userRepository.GetUserByUsername(username);

            if (user == null || user.Password != password)
                throw new ArgumentException("Invalid username or password.");

            if (user.AccountStatus != "Active")
                throw new InvalidOperationException("User account is not active.");

            var userDto = _mapper.Map<GetUserDTOs>(user);
            //userDto.Roles = user.Roles.Select(r => r.RoleName).ToList();

            return userDto;
        }

        public async Task<GetUserDTOs> Register(RegisterUserDTOs registerUserDTOs)
        {
            if (registerUserDTOs.Password != registerUserDTOs.ConfirmPassword)
                throw new ArgumentException("Password and confirm password do not match.");

            var existingUserByUsername = await _userRepository.GetUserByUsername(registerUserDTOs.Username);
            if (existingUserByUsername != null)
                throw new ArgumentException("Username already exists.");

            var existingUserByEmail = await _userRepository.GetUserByEmail(registerUserDTOs.Email);
            if (existingUserByEmail != null)
                throw new ArgumentException("Email already exists.");

            var user = _mapper.Map<User>(registerUserDTOs);
            user.AccountStatus = "Inactive";

            if (user.Roles == null)
            {
                user.Roles = new List<Role>();
            }

            var customerRole = await _roleRepository.GetRoleById(3);
            if (customerRole == null)
            {
                throw new InvalidOperationException("Role with RoleId = 3 does not exist.");
            }
            user.Roles.Add(customerRole);

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return _mapper.Map<GetUserDTOs>(user);
        }
    }
}
