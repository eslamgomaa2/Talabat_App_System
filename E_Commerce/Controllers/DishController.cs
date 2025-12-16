using Domin.DTOS.DTO;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;

namespace E_Commerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DishController : ControllerBase
    {
        private readonly IDishesServices _dishesServices;

        public DishController(IDishesServices dishesServices)
        {
            _dishesServices = dishesServices;
        }

        [HttpGet("GetAllDishes")]
        public async Task<IActionResult> GetAllDishes()
        {
            var dishes = await _dishesServices.GetAllDishes();
            return Ok(dishes);
        }

        [HttpPost("AddDish")]
        public async Task<IActionResult> AddDish([FromForm] DishDto request)
        {
            var dishes = await _dishesServices.AddaDishForASpecificRrestaurant(request);
            return Ok(dishes);
        }

        [HttpPut("UpdateDish/{id}")]
        public async Task<IActionResult> UpdateDish([FromRoute] int id, [FromForm] DishDto request)
        {
            var dishes = await _dishesServices.UpdateDishes(id, request);
            return Ok(dishes);
        }

        [HttpDelete("DeleteDish/{id}")]
        public async Task<IActionResult> DeleteDish([FromRoute] int id)
        {
            var dishes = await _dishesServices.DeletDish(id);
            return Ok(dishes);
        }

        [HttpGet("GetDishById/{id}")]
        public async Task<IActionResult> GetDishById([FromRoute] int id)
        {
            var dish = await _dishesServices.GetDishById(id);
            return Ok(dish);
        }

        [HttpGet("GetDishesByName")]
        public async Task<IActionResult> GetDishesByName([FromQuery] string name)
        {
            var dishes = await _dishesServices.GetDishesByName(name);
            return Ok(dishes);
        }

        [HttpGet("GetAvailableDishes/{id}")]
        public async Task<IActionResult> GetOnlyAvailableDishes([FromRoute] int id)
        {
            var dishes = await _dishesServices.GetOnlyAvailableDishesForRestaurant(id);
            return Ok(dishes);
        }
    }
}
