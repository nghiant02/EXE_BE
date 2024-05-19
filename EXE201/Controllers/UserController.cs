using EXE201.BLL.Interfaces;
using EXE201.DAL.DTOs.UserDTOs;
using EXE201.ViewModel.UserViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : Controller
    {
        private readonly IUserServices _userServices;

        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
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
    }
}
