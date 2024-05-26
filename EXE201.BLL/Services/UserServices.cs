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
        private readonly IVerifyCodeRepository _verifyCodeRepository;

        public UserServices(IUserRepository userRepository, IMapper mapper, IRoleRepository roleRepository, IVerifyCodeRepository verifyCodeRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _roleRepository = roleRepository;
            _verifyCodeRepository = verifyCodeRepository;
        }

        public async Task<User> AddUserForStaff(AddNewUserDTO addNewUserDTO)
        {
            var mapUser = _mapper.Map<User>(addNewUserDTO);

            await _userRepository.AddNewUser(mapUser);
            return mapUser;
        }

        public async Task<User> ChangePasword(int id, ChangePasswordDTO changePasswordDTO)
        {
            var users = await _userRepository.FindAsync(x => x.UserId == id && x.Password == changePasswordDTO.CurrentPassword);
            if (!users.Any())
            {
                throw new ArgumentException("Wrong Password!");
            }
            var user = users.First();
            user.Password = changePasswordDTO.NewPassword;
            return await _userRepository.UpdateUser(user);
        }

        public async Task<bool> ChangeStatusUserToNotActive(int userId)
        {
            var existUser = await _userRepository.GetUserById(userId);
            if (existUser == null)
            {
                throw new ArgumentException("Id does not exist!!");
            }
            await _userRepository.ChangeStatusUserToNotActive(existUser.UserId);
            return true;
        }

        public async Task<User> FindUserByEmail(string email)
        {
            var user = await _userRepository.FindAsync(x => x.Email == email);
            if (!user.Any())
            {
                return null;
            }
            return user.First();
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

        public async Task<User> UpdatePassword(string email, string password, int id)
        {
            var checkCode = await _verifyCodeRepository.FindAsync(x => x.Email == email);
            var user = await _userRepository.FindAsync(x => x.Email == email && x.UserId == id);
            if (!user.Any() || !checkCode.Any())
            {
                throw new Exception("Invalid Request");
            }
            var existUser = user.First();
            var verifyCode = checkCode.First();

            var updateUser = await _userRepository.UpdateUser(existUser);
            await _verifyCodeRepository.Delete(verifyCode);
            return updateUser;
        }

        public async Task<User> UserUpdateUser(int id, UpdateProfileUserDTO userView)
        {
            var oldUser = await _userRepository.FindAsync(x => x.UserId == id);
            if (!oldUser.Any())
            {
                throw new ArgumentException($"User with ID {id} not found");
            }
            var updatingUser = _mapper.Map<User>(userView);
            updatingUser.UserId = id;
            updatingUser.Phone = oldUser.First().Phone;
            updatingUser.Email = oldUser.First().Email;
            updatingUser.AccountStatus = oldUser.First().AccountStatus;
            updatingUser.Password = oldUser.First().Password;

            return await _userRepository.UpdateUser(updatingUser);
        }
    }
}
