using Microsoft.AspNetCore.Mvc;
using RestaurantFoodOrderSystem.Models;
using RestaurantFoodOrderSystem.Requests;
using RestaurantFoodOrderSystem.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantFoodOrderSystem.Controllers
{
	[Route("api/restaurants")]
	[ApiController]
	public class RestaurantController : ControllerBase
	{
		private readonly RestaurantService _restaurantService;
		private readonly UserService _userService;

		public RestaurantController(RestaurantService restaurantService, UserService userService)
		{
			_restaurantService = restaurantService;
			_userService = userService;
		}

		[HttpGet("search")]
		public async Task<IActionResult> SearchRestaurant([FromHeader(Name = "Authorization")] string jwt,
														  [FromQuery] string keyword)
		{
			var user = await _userService.FindUserByJwtToken(jwt);
			var restaurants = await _restaurantService.SearchRestaurants(keyword);
			return Ok(restaurants);
		}

		[HttpGet]
		public async Task<IActionResult> GetAllRestaurants([FromHeader(Name = "Authorization")] string jwt)
		{
			var user = await _userService.FindUserByJwtToken(jwt);
			var restaurants = await _restaurantService.GetAllRestaurants();
			return Ok(restaurants);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> FindRestaurantById([FromHeader(Name = "Authorization")] string jwt,
															 [FromRoute] long id)
		{
			var user = await _userService.FindUserByJwtToken(jwt);
			var restaurant = await _restaurantService.FindRestaurantById(id);
			return Ok(restaurant);
		}

		[HttpPut("{id}/add-favorites")]
		public async Task<IActionResult> AddToFavorites([FromHeader(Name = "Authorization")] string jwt,
														 [FromRoute] long id)
		{
			var user = await _userService.FindUserByJwtToken(jwt);
			var restaurantDto = await _restaurantService.AddToFavorites(id, user);
			return Ok(restaurantDto);
		}
	}
}
