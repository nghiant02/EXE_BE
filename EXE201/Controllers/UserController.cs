using EXE201.BLL.Interfaces;
using EXE201.DAL.DTOs.UserDTOs;
using EXE201.ViewModel.UserViewModel;
using Microsoft.AspNetCore.Mvc;
using EXE201.DAL.DTOs.EmailDTOs;
using System.Text;
using EXE201.BLL.Services;
using System.ComponentModel.DataAnnotations;

namespace EXE201.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly IEmailService _emailService;

        public UserController(IUserServices userServices, IEmailService emailService)
        {
            _userServices = userServices;
            _emailService = emailService;
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
                return Ok(result);
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
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserDTOs registerUserDTOs)
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

        [HttpPost("SendMail")]
        public async Task<IActionResult> SendMail(EmailView emailView)
        {
            try
            {
                EmailDTO emailDTO = new EmailDTO();
                emailDTO.To = emailView.To;
                emailDTO.Subject = "Confirm Account";
                emailDTO.Body = GetHtmlcontent(emailView.Name);
                await _emailService.SendEmail(emailDTO);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpPut("Change-Password")]
        public async Task<IActionResult> UpdatePassword(int id, ChangePasswordDTO changePassword)
        {
            var user = await _userServices.ChangePasword(id,changePassword);
            return Ok(user);
        }

        [HttpPut("Update-Profile")]
        public async Task<IActionResult> UpdateUser([FromQuery][Required] int id, UpdateProfileUserDTO userView)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userServices.UserUpdateUser(id, userView);
            return Ok(user);
        }

        private string GetHtmlcontent(string name)
        {
            string Response = "<div style=\"wid" +
                              "th:100%;text-align:center;margin:10px\">";
            Response += "<h1>Welcome to Voguary</h1>";
            Response +=
                "<img style=\"width:10rem\" src=\"https://cdn-icons-png.flaticon.com/128/1145/1145941.png\" />";
            Response += "<h1 style=\"color:#f57f0e\">Dear " + name + "</h1>";
            Response +=
                "<button style=\"background-color: #f57f0e; color: white; padding: 14px 20px; margin: 8px 0; border: none; cursor: pointer; border-radius: 4px;\">";
            Response += "<a href=\" " + name +
                        "\" style=\"text-decoration: none; color: white;\">Activate the account</a>";
            Response += "</button>";
            Response += "<div><h1>Contact us: lammjnhphong4560@gmail.com</h1></div>";
            Response += "</div>";
            return Response;
        }

        [HttpGet("GetFilteredUser")]
        public async Task<IActionResult> GetFilteredUser([FromQuery] UserFilterDTO filter)
        {
            var users = await _userServices.GetFilteredUser(filter);
            return Ok(users);
        }
    }
}
