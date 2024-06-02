using EXE201.BLL.Interfaces;
using EXE201.DAL.DTOs.UserDTOs;
using EXE201.ViewModel.UserViewModel;
using Microsoft.AspNetCore.Mvc;
using EXE201.DAL.DTOs.EmailDTOs;
using System.Text;
using EXE201.BLL.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace EXE201.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly IEmailService _emailService;
        private readonly IJwtService _jwtService;

        public UserController(IUserServices userServices, IEmailService emailService, IJwtService jwtService)
        {
            _userServices = userServices;
            _emailService = emailService;
            _jwtService = jwtService;
        }

        [HttpGet("GetAllProfileUsers")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _userServices.GetAllProfileUser();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUserViewModel loginUserViewModel)
        {
            try
            {
                var result = await _userServices.Login(loginUserViewModel.Username, loginUserViewModel.Password);

                if (result == null)
                {
                    return Unauthorized("Invalid username or password.");
                }

                var token = _jwtService.GenerateToken(result.UserId.ToString());

                return Ok(new { Token = token, User = result });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddNewUserForStaff")]
        public async Task<IActionResult> AddNewUser(AddNewUserDTO addNewUserDTO)
        {
            try
            {
                var result = await _userServices.AddUserForStaff(addNewUserDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterAsync(
            [FromBody] RegisterUserDTOs registerUserDTOs)
        {
            try
            {
                var result = await _userServices.Register(registerUserDTOs);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("BlockUser")]
        public async Task<IActionResult> BlockUser(int userId)
        {
            try
            {
                var result = await _userServices.ChangeStatusUserToNotActive(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Change-Password")]
        public async Task<IActionResult> UpdatePassword(
            int id, ChangePasswordDTO changePassword)
        {
            var user = await _userServices.ChangePasword(id, changePassword);
            return Ok(user);
        }

        [HttpPut("Update-Profile")]
        public async Task<IActionResult> UpdateUser([FromQuery] [Required] int id, UpdateProfileUserDTO userView)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userServices.UserUpdateUser(id, userView);
            return Ok(user);
        }

        [HttpGet("GetFilteredUser")]
        public async Task<IActionResult> GetFilteredUser(
            [FromQuery] UserFilterDTO filter)
        {
            var users = await _userServices.GetFilteredUser(filter);
            return Ok(users);
        }

        [HttpGet("ViewProfile/{userId}")]
        public async Task<IActionResult> ViewProfile(int userId)
        {
            var userProfile = await _userServices.GetUserProfile(userId);
            if (userProfile == null)
            {
                return NotFound();
            }
            return Ok(userProfile);
        }
    }
}
