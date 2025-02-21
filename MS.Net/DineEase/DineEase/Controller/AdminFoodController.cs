using Microsoft.AspNetCore.Mvc;
using RestaurantFoodOrderSystem.Models;
using RestaurantFoodOrderSystem.Requests;
using RestaurantFoodOrderSystem.Responses;
using RestaurantFoodOrderSystem.Services;
using System.Threading.Tasks;

namespace RestaurantFoodOrderSystem.Controllers
{
    [Route("api/admin/food")]
    [ApiController]
    public class AdminFoodController : ControllerBase
    {
        private readonly FoodService _foodService;
        private readonly UserService _userService;
        private readonly RestaurantService _restaurantService;

        public AdminFoodController(FoodService foodService, UserService userService, RestaurantService restaurantService)
        {
            _foodService = foodService;
            _userService = userService;
            _restaurantService = restaurantService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFood([FromBody] CreateFoodRequest request, [FromHeader(Name = "Authorization")] string jwt)
        {
            var user = await _userService.FindUserByJwtTokenAsync(jwt);
            var restaurant = await _restaurantService.FindRestaurantByIdAsync(request.RestaurantId);

            var food = await _foodService.CreateFoodAsync(request, request.Category, restaurant);
            return CreatedAtAction(nameof(CreateFood), new { id = food.Id }, food);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFood(long id, [FromHeader(Name = "Authorization")] string jwt)
        {
            var user = await _userService.FindUserByJwtTokenAsync(jwt);
            await _foodService.DeleteFoodAsync(id);

            var messageResponse = new MessageResponse { Message = "Food deleted successfully" };
            return Ok(messageResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFoodAvailabilityStatus(long id, [FromHeader(Name = "Authorization")] string jwt)
        {
            var user = await _userService.FindUserByJwtTokenAsync(jwt);
            var food = await _foodService.UpdateAvailabilityStatusAsync(id);

            return Ok(food);
        }
    }
}
