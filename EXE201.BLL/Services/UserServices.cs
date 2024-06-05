// EXE201.BLL.Services.UserServices.cs
using AutoMapper;
using EXE201.BLL.DTOs.UserDTOs;
using EXE201.BLL.Interfaces;
using EXE201.DAL.DTOs.EmailDTOs;
using EXE201.DAL.DTOs.UserDTOs;
using EXE201.DAL.Interfaces;
using EXE201.DAL.Models;
using EXE201.DAL.Repository;
using LMSystem.Repository.Helpers;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Tools;


namespace EXE201.BLL.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;
        private readonly IEmailService _emailService;
        private readonly IVerifyCodeRepository _verifyCodeRepository;

        public UserServices(IUserRepository userRepository, IMapper mapper, IRoleRepository roleRepository, IVerifyCodeRepository verifyCodeRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _roleRepository = roleRepository;
            _verifyCodeRepository = verifyCodeRepository;
            _emailService = emailService;
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

        public async Task<PagedList<UserListDTO>> GetFilteredUser(UserFilterDTO filter)
        {
            return await _userRepository.GetFilteredUser(filter);
        }

        public async Task<UserProfileDTO> GetUserProfile(int userId)
        {
            return await _userRepository.GetUserProfile(userId);
        }

        public async Task<GetUserDTOs> Login(string username, string password)
        {
            var user = await _userRepository.GetUserByUsername(username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
                throw new ArgumentException("Invalid username or password.");

            if (user.AccountStatus != "Active")
                throw new InvalidOperationException("User account is not active.");

            var userDto = _mapper.Map<GetUserDTOs>(user);

            return userDto;
        }

        public async Task<(bool Success, int UserId)> RegisterUserAsync(RegisterUserRequest request)
        {
            // Check if the username already exists
            var existingUser = await _userRepository.GetUserByUsername(request.UserName);
            if (existingUser != null)
            {
                throw new ArgumentException("User Name already exists");
            }

            // Check if the email already exists
            var existingEmail = await _userRepository.FindAsync(x => x.Email == request.Email);
            if (existingEmail.Any())
            {
                throw new ArgumentException("Email already exists");
            }

            var user = new User
            {
                UserName = request.UserName,
                FullName = request.FullName,
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                AccountStatus = "Inactive"
            };

            // Set default role for user
            var customerRole = await _roleRepository.GetRoleById(3);
            if (customerRole != null)
            {
                user.Roles.Add(customerRole);
            }

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            var verificationCode = new Random().Next(100000, 999999).ToString();
            var verifyCode = new VerifyCode
            {
                Id = IdGenerator.GenerateId(),
                UserId = user.UserId,
                Email = user.Email,
                Code = verificationCode,
                CreatedAt = DateTime.Now
            };

            await _verifyCodeRepository.AddAsync(verifyCode);
            await _verifyCodeRepository.SaveChangesAsync();

            var emailSent = await _emailService.SendEmailAsync(new EmailDTO
            {
                ToEmail = user.Email,
                Subject = "Verify your email",
                Body = $"Please verify your email by entering this code in the app: {verificationCode}"
            });

            return (emailSent, user.UserId);
        }

        public async Task<bool> VerifyEmailWithCodeAsync(int userId, string code)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var verifyCode = await _verifyCodeRepository.FindAsync(v => v.UserId == userId && v.Code == code);
            if (!verifyCode.Any())
            {
                return false;
            }

            user.AccountStatus = "Active";
            await _userRepository.SaveChangesAsync();

            await _verifyCodeRepository.Delete(verifyCode.First());

            return true;
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

            existUser.Password = password;
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
            updatingUser.UserName = oldUser.First().UserName;
            updatingUser.AccountStatus = oldUser.First().AccountStatus;
            updatingUser.Password = oldUser.First().Password;
            updatingUser.ProfileImage = oldUser.First().ProfileImage;

            var address = oldUser.First().Addresses.FirstOrDefault();
            if (address != null)
            {
                address.Street = userView.Street;
                address.City = userView.City;
                address.State = userView.State;
                address.PostalCode = userView.PostalCode;
                address.Country = userView.Country;
            }
            else
            {
                address = new Address
                {
                    UserId = id,
                    Street = userView.Street,
                    City = userView.City,
                    State = userView.State,
                    PostalCode = userView.PostalCode,
                    Country = userView.Country
                };
                updatingUser.Addresses.Add(address);
            }
            return await _userRepository.UpdateUser(updatingUser);
        }
    }
}
