using Microsoft.AspNetCore.Mvc;

namespace RestaurantFoodOrderSystem.Controllers
{
	[Route("/")]
	[ApiController]
	public class HomeController : ControllerBase
	{
		[HttpGet]
		public IActionResult Get()
		{
			return Ok("Welcome to food delivery World");
		}
	}
}
