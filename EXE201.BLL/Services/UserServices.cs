using AutoMapper;
using EXE201.BLL.Interfaces;
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

        public UserServices(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<User> Login(string username, string password)
        {
            // Attempt to retrieve the user by username
            var user = await _userRepository.GetUserByUsername(username);

            // Validate user existence
            if (user == null)
            {
                // Optionally log this event for audit purposes
                throw new ArgumentException("User not found.");
            }

            // Validate password
            if (user.Password != password)
            {
                // Optionally log this event as a failed login attempt
                throw new ArgumentException("Invalid password provided.");
            }

            // Check if the user account is active
            if (user.Status != "Active")
            {
                // You might want to log this as a security or business rule enforcement
                throw new InvalidOperationException("User account is not active.");
            }

            // All validations passed, return the mapped user DTO
            return _mapper.Map<User>(user);
        }

    }
}
