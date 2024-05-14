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

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUserViewModel loginUserViewModel)
        {
            try
            {
                var result = await _userServices.Login(loginUserViewModel.Username, loginUserViewModel.Password);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                // Log the error message here if needed
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                // Use BadRequest for business rules violations as well
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // For any other unhandled exceptions, use a generic error message
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred processing your request.");
            }
        }
    }
}
