using Microsoft.AspNetCore.Mvc;
using RestaurantFoodOrderSystem.Models;
using RestaurantFoodOrderSystem.Requests;
using RestaurantFoodOrderSystem.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantFoodOrderSystem.Controllers
{
    [Route("api/admin/ingredients")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly IngredientsService _ingredientsService;

        public IngredientController(IngredientsService ingredientsService)
        {
            _ingredientsService = ingredientsService;
        }

        [HttpPost("category")]
        public async Task<IActionResult> CreateIngredientCategory([FromBody] IngredientCategoryRequest req)
        {
            var item = await _ingredientsService.CreateIngredientCategory(req.Name, req.RestaurantId);
            return CreatedAtAction(nameof(GetRestaurantIngredientCategory), new { id = item.Id }, item);
        }

        [HttpPost]
        public async Task<IActionResult> CreateIngredientItem([FromBody] IngredientRequest req)
        {
            var item = await _ingredientsService.CreateIngredientsItem(req.RestaurantId, req.Name, req.CategoryId);
            return CreatedAtAction(nameof(GetRestauranIngredient), new { id = item.RestaurantId }, item);
        }

        [HttpPut("{id}/stock")]
        public async Task<IActionResult> UpdateIngredientStock([FromRoute] long id)
        {
            var item = await _ingredientsService.UpdateStock(id);
            return Ok(item);
        }

        [HttpGet("restaurant/{id}")]
        public async Task<IActionResult> GetRestaurantIngredient([FromRoute] long id)
        {
            var items = await _ingredientsService.FindRestaurantIngredients(id);
            return Ok(items);
        }

        [HttpGet("restaurant/{id}/category")]
        public async Task<IActionResult> GetRestaurantIngredientCategory([FromRoute] long id)
        {
            var items = await _ingredientsService.FindIngredientCategoryByRestaurantId(id);
            return Ok(items);
        }
    }
}
