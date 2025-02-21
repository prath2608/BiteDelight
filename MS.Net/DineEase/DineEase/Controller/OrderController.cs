using Microsoft.AspNetCore.Mvc;
using RestaurantFoodOrderSystem.Models;
using RestaurantFoodOrderSystem.Requests;
using RestaurantFoodOrderSystem.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantFoodOrderSystem.Controllers
{
    [Route("api")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly UserService _userService;

        public OrderController(OrderService orderService, UserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }

        [HttpPost("order")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequest req,
                                                     [FromHeader(Name = "Authorization")] string jwt)
        {
            var user = await _userService.FindUserByJwtToken(jwt);
            var order = await _orderService.CreateOrder(req, user);
            return CreatedAtAction(nameof(GetOrderHistory), new { userId = user.Id }, order);
        }

        [HttpGet("order/user")]
        public async Task<IActionResult> GetOrderHistory([FromHeader(Name = "Authorization")] string jwt)
        {
            var user = await _userService.FindUserByJwtToken(jwt);
            var orders = await _orderService.GetUsersOrder(user.Id);
            return Ok(orders);
        }
    }
}
