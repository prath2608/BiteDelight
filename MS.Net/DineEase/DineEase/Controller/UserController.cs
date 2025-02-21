using Microsoft.AspNetCore.Mvc;
using RestaurantFoodOrderSystem.Models;
using RestaurantFoodOrderSystem.Services;
using System.Threading.Tasks;

namespace RestaurantFoodOrderSystem.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> FindUserByJwtToken([FromHeader(Name = "Authorization")] string jwt)
        {
            try
            {
                var user = await _userService.FindUserByJwtToken(jwt);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
