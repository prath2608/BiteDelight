using Microsoft.AspNetCore.Mvc;
using RestaurantFoodOrderSystem.Models;
using RestaurantFoodOrderSystem.Requests;
using RestaurantFoodOrderSystem.Services;
using RestaurantFoodOrderSystem.Repositories;

namespace RestaurantFoodOrderSystem.Controllers
{
    [Route("api")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;
        private readonly UserService _userService;

        public CartController(CartService cartService, UserService userService)
        {
            _cartService = cartService;
            _userService = userService;
        }

        [HttpPut("cart/add")]
        public IActionResult AddItemToCart([FromBody] AddCartItemRequest req, [FromHeader(Name = "Authorization")] string jwt)
        {
            var cartItem = _cartService.AddItemToCart(req, jwt);
            return Ok(cartItem);
        }

        [HttpPut("cart-item/update")]
        public IActionResult UpdateCartItemQuantity([FromBody] UpdateCartItemRequest req, [FromHeader(Name = "Authorization")] string jwt)
        {
            var cartItem = _cartService.UpdateCartItemQuantity(req.CartItemId, req.Quantity);
            return Ok(cartItem);
        }

        [HttpDelete("cart-item/{id}/remove")]
        public IActionResult RemoveCartItem([FromRoute] long id, [FromHeader(Name = "Authorization")] string jwt)
        {
            var cart = _cartService.RemoveItemFromCart(id, jwt);
            return Ok(cart);
        }

        [HttpDelete("cart/clear")]
        public IActionResult ClearCart([FromHeader(Name = "Authorization")] string jwt)
        {
            var user = _userService.FindUserByJwtToken(jwt);
            var cart = _cartService.ClearCart(user.Id);
            return Ok(cart);
        }

        [HttpGet("cart")]
        public IActionResult FindUserCart([FromHeader(Name = "Authorization")] string jwt)
        {
            var user = _userService.FindUserByJwtToken(jwt);
            var cart = _cartService.FindCartByUserId(user.Id);
            return Ok(cart);
        }
    }
}
