using AutoMapper;
using EXE201.BLL.DTOs.UserDTOs;
using EXE201.BLL.Interfaces;
using EXE201.DAL.Interfaces;
using EXE201.DAL.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

        public UserServices(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
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

        public Task<IEnumerable<User>> GetAllProfileUser()
        {
            var allUser = _userRepository.GetAllUsers();
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

            if (user.Status != "Active")
                throw new InvalidOperationException("User account is not active.");

            return _mapper.Map<GetUserDTOs>(user);
        }

    }
}
