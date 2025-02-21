using Microsoft.AspNetCore.Mvc;
using RestaurantFoodOrderSystem.Models;
using RestaurantFoodOrderSystem.Requests;
using RestaurantFoodOrderSystem.Responses;
using RestaurantFoodOrderSystem.Services;
using System.Threading.Tasks;

namespace RestaurantFoodOrderSystem.Controllers
{
    [Route("api/admin/restaurants")]
    [ApiController]
    public class AdminRestaurantController : ControllerBase
    {
        private readonly RestaurantService _restaurantService;
        private readonly UserService _userService;

        public AdminRestaurantController(RestaurantService restaurantService, UserService userService)
        {
            _restaurantService = restaurantService;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantRequest req, [FromHeader(Name = "Authorization")] string jwt)
        {
            var user = await _userService.FindUserByJwtTokenAsync(jwt);
            var restaurant = await _restaurantService.CreateRestaurantAsync(req, user);
            return CreatedAtAction(nameof(GetRestaurantByUserId), new { id = restaurant.Id }, restaurant);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRestaurant([FromBody] CreateRestaurantRequest req, [FromHeader(Name = "Authorization")] string jwt, long id)
        {
            var user = await _userService.FindUserByJwtTokenAsync(jwt);
            var restaurant = await _restaurantService.UpdateRestaurantAsync(id, req);
            return Ok(restaurant);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurant([FromHeader(Name = "Authorization")] string jwt, long id)
        {
            var user = await _userService.FindUserByJwtTokenAsync(jwt);
            await _restaurantService.DeleteRestaurantAsync(id);
            var response = new MessageResponse { Message = "Restaurant deleted successfully" };
            return Ok(response);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateRestaurantStatus([FromHeader(Name = "Authorization")] string jwt, long id)
        {
            var user = await _userService.FindUserByJwtTokenAsync(jwt);
            var restaurant = await _restaurantService.UpdateRestaurantStatusAsync(id);
            return Ok(restaurant);
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetRestaurantByUserId([FromHeader(Name = "Authorization")] string jwt)
        {
            var user = await _userService.FindUserByJwtTokenAsync(jwt);
            var restaurant = await _restaurantService.GetRestaurantByUserIdAsync(user.Id);
            return Ok(restaurant);
        }
    }
}
