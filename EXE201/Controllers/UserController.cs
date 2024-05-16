using EXE201.BLL.Interfaces;
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

        [HttpGet("GetAllProfileUsers")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _userServices.GetAllProfileUser();
                return Ok(result);
            }catch (Exception ex)
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

        [HttpPost("BlockUser")]
        public async Task<IActionResult> BlockUser(int userId)
        {
            try
            {
                var result = await _userServices.ChangeStatusUserToNotActive(userId);
                return Ok(result);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
