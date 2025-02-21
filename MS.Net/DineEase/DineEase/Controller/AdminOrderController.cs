using Microsoft.AspNetCore.Mvc;
using RestaurantFoodOrderSystem.Models;
using RestaurantFoodOrderSystem.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantFoodOrderSystem.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminOrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly UserService _userService;

        public AdminOrderController(OrderService orderService, UserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }

        [HttpGet("order/restaurant/{id}")]
        public async Task<IActionResult> GetOrderHistory(long id, [FromQuery] string orderStatus, [FromHeader(Name = "Authorization")] string jwt)
        {
            var user = await _userService.FindUserByJwtTokenAsync(jwt);
            var orders = await _orderService.GetRestaurantOrderAsync(id, orderStatus);
            return Ok(orders);
        }

        [HttpPut("order/{id}/{orderStatus}")]
        public async Task<IActionResult> UpdateOrderStatus(long id, string orderStatus, [FromHeader(Name = "Authorization")] string jwt)
        {
            var user = await _userService.FindUserByJwtTokenAsync(jwt);
            var order = await _orderService.UpdateOrderAsync(id, orderStatus);
            return Ok(order);
        }
    }
}
