using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestaurantFoodOrderSystem.Models;
using RestaurantFoodOrderSystem.Requests;
using RestaurantFoodOrderSystem.Responses;
using RestaurantFoodOrderSystem.Services;
using RestaurantFoodOrderSystem.Config;
using RestaurantFoodOrderSystem.Repositories;

namespace RestaurantFoodOrderSystem.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly JwtProvider _jwtProvider;
        private readonly CustomerUserDetailsService _customerUserDetailsService;
        private readonly CartRepository _cartRepository;

        public AuthController(UserRepository userRepository, PasswordHasher<User> passwordHasher, JwtProvider jwtProvider,
            CustomerUserDetailsService customerUserDetailsService, CartRepository cartRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
            _customerUserDetailsService = customerUserDetailsService;
            _cartRepository = cartRepository;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> CreateUserHandler([FromBody] User user)
        {
            var isEmailExist = await _userRepository.FindByEmailAsync(user.Email);
            if (isEmailExist != null)
            {
                return BadRequest("Email is already used with another account");
            }

            var createdUser = new User
            {
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role,
                Password = _passwordHasher.HashPassword(user, user.Password)
            };

            var savedUser = await _userRepository.SaveAsync(createdUser);

            var cart = new Cart { Customer = savedUser };
            await _cartRepository.SaveAsync(cart);

            var jwt = _jwtProvider.GenerateToken(savedUser);

            var authResponse = new AuthResponse
            {
                Jwt = jwt,
                Message = "Register success",
                Role = savedUser.Role.ToString()
            };

            return CreatedAtAction(nameof(SignIn), authResponse);
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] LoginRequest req)
        {
            var username = req.Email;
            var password = req.Password;

            var authentication = Authenticate(username, password);

            var role = authentication.IsAuthenticated ? authentication.Principal.FindFirst("role")?.Value : null;

            var jwt = _jwtProvider.GenerateToken(authentication.Principal);

            var authResponse = new AuthResponse
            {
                Jwt = jwt,
                Message = "Login success",
                Role = role
            };

            return Ok(authResponse);
        }

        private AuthenticationResult Authenticate(string username, string password)
        {
            var userDetails = _customerUserDetailsService.LoadUserByUsername(username);
            if (userDetails == null || !_passwordHasher.VerifyHashedPassword(userDetails, password))
            {
                return new AuthenticationResult { IsAuthenticated = false };
            }

            var authentication = new AuthenticationResult
            {
                IsAuthenticated = true,
                Principal = new ClaimsPrincipal(new ClaimsIdentity(userDetails.GetClaims(), "Bearer"))
            };

            return authentication;
        }
    }
}
