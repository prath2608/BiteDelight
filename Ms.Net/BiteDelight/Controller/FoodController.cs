using Microsoft.AspNetCore.Mvc;
using RestaurantFoodOrderSystem.Models;
using RestaurantFoodOrderSystem.Services;
using RestaurantFoodOrderSystem.Requests;
using System.Collections.Generic;

namespace RestaurantFoodOrderSystem.Controllers
{
    [Route("api/food")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly FoodService _foodService;
        private readonly UserService _userService;
        private readonly RestaurantService _restaurantService;

        public FoodController(FoodService foodService, UserService userService, RestaurantService restaurantService)
        {
            _foodService = foodService;
            _userService = userService;
            _restaurantService = restaurantService;
        }

        [HttpGet("search")]
        public IActionResult SearchFood([FromQuery] string name, [FromHeader(Name = "Authorization")] string jwt)
        {
            var user = _userService.FindUserByJwtToken(jwt);
            var foods = _foodService.SearchFood(name);
            return Ok(foods);
        }

        [HttpGet("restaurant/{restaurantId}")]
        public IActionResult GetRestaurantFood(
            [FromQuery] bool vegatarian,
            [FromQuery] bool seasonal,
            [FromQuery] bool nonveg,
            [FromQuery] string food_category,
            [FromRoute] long restaurantId,
            [FromHeader(Name = "Authorization")] string jwt)
        {
            var user = _userService.FindUserByJwtToken(jwt);
            var foods = _foodService.GetRestaurantsFood(restaurantId, vegatarian, nonveg, seasonal, food_category);
            return Ok(foods);
        }
    }
}
