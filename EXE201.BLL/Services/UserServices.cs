using AutoMapper;
using EXE201.BLL.DTOs.UserDTOs;
using EXE201.BLL.Interfaces;
using EXE201.DAL.DTOs.EmailDTOs;
using EXE201.DAL.DTOs;
using EXE201.DAL.DTOs.UserDTOs;
using EXE201.DAL.Interfaces;
using EXE201.DAL.Models;
using EXE201.DAL.Repository;
using LMSystem.Repository.Helpers;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        private readonly IJwtService _jwtService;

        public UserServices(IUserRepository userRepository, IMapper mapper, IRoleRepository roleRepository, IEmailService emailService, IVerifyCodeRepository verifyCodeRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _roleRepository = roleRepository;
            _emailService = emailService;
            _verifyCodeRepository = verifyCodeRepository;
            _jwtService = jwtService;
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

        public async Task<PagedResponseDTO<UserListDTO>> GetFilteredUser(UserFilterDTO filter)
        {
            return await _userRepository.GetFilteredUser(filter);
        }

        public async Task<UserProfileDTO> GetUserProfile(int userId)
        {
            return await _userRepository.GetUserProfile(userId);
        }

        public async Task<(bool Success, int UserId)> RegisterUserAsync(RegisterUserRequest request)
        {
            var existingUser = await _userRepository.GetUserByUsername(request.UserName);
            if (existingUser != null)
            {
                throw new ArgumentException("User Name already exists");
            }

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
                UserStatus = "Inactive"
            };

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

            user.UserStatus = "Active";
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

        public async Task<User> UserUpdateAvartar(int id, UpdateAvatarUserDTO updateAvatarUserDTO)
        {
            var checkId = await _userRepository.FindAsync(x => x.UserId == id);
            if (!checkId.Any())
            {
                throw new ArgumentException($"User with ID {id} not found");
            }
            var updateUser = _mapper.Map<User>(updateAvatarUserDTO);
            updateUser.UserId = id;
            updateUser.UserName = checkId.First().UserName;
            updateUser.UserStatus = checkId.First().UserStatus;
            updateUser.Password = checkId.First().Password;
            return await _userRepository.UpdateUser(updateUser);
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
            updatingUser.UserStatus = oldUser.First().UserStatus;
            updatingUser.Password = oldUser.First().Password;
            updatingUser.ProfileImage = oldUser.First().ProfileImage;
            updatingUser.HouseNumber = oldUser.First().HouseNumber;
            updatingUser.StreetName = oldUser.First().StreetName;
            updatingUser.District = oldUser.First().District;
            updatingUser.CityProvince = oldUser.First().CityProvince;

            return await _userRepository.UpdateUser(updatingUser);
        }

        public async Task<LoginResponseDTOs> Login(string username, string password)
        {
            var user = await _userRepository.GetUserByUsername(username);

            if (user == null)
                throw new ArgumentException("Invalid username or password.");

            bool passwordIsValid = false;

            try
            {
                passwordIsValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
            }
            catch
            {
                passwordIsValid = user.Password == password;
            }

            if (!passwordIsValid)
                throw new ArgumentException("Invalid username or password.");

            if (user.UserStatus != "Active")
                throw new InvalidOperationException("User account is not active.");

            if (user.Password == password)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(password);
                await _userRepository.UpdateUser(user);
            }

            var roles = user.Roles.Select(r => r.RoleName).ToList();
            var token = _jwtService.GenerateToken(user.UserId.ToString(), user.UserName, user.Email, roles);
            var refreshToken = _jwtService.GenerateRefreshToken();

            var tokenEntity = new Token
            {
                UserId = user.UserId,
                AccessToken = token,
                RefreshToken = refreshToken,
                IssuedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(30),
                Status = "Active"
            };
            await _userRepository.UpdateToken(tokenEntity);

            var expirationDate = DateTime.UtcNow.AddMinutes(30).ToString("yyyy-MM-ddTHH:mm:ssZ");

            var response = new LoginResponseDTOs
            {
                Token = token,
                RefreshToken = refreshToken,
                Expired = expirationDate
            };

            return response;
        }

        public async Task<(string Token, string RefreshToken)> RefreshTokenAsync(string token, string refreshToken)
        {
            var principal = _jwtService.GetPrincipalFromExpiredToken(token);
            var userId = principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

            var savedToken = await _userRepository.GetRefreshTokenByUserId(userId);
            if (savedToken.RefreshToken != refreshToken || savedToken.ExpiresAt <= DateTime.UtcNow)
            {
                throw new SecurityTokenException("Invalid refresh token");
            }

            var user = await _userRepository.GetUserById(int.Parse(userId));
            var roles = user.Roles.Select(r => r.RoleName).ToList();
            var newJwtToken = _jwtService.GenerateToken(user.UserId.ToString(), user.UserName, user.Email, roles);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            savedToken.RefreshToken = newRefreshToken;
            savedToken.IssuedAt = DateTime.UtcNow;
            savedToken.ExpiresAt = DateTime.UtcNow.AddMinutes(30);
            await _userRepository.UpdateToken(savedToken);

            return (newJwtToken, newRefreshToken);
        }
    }
}
