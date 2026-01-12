using Domin.DTOS.DTO;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;

namespace E_Commerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantServices _restaurantServices;

        public RestaurantController(IRestaurantServices restaurantServices)
        {
            _restaurantServices = restaurantServices;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllRestaurants()
        {
            var restaurants = await _restaurantServices.GetAllRestaurants();
            return Ok(restaurants);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddRestaurantAsync([FromBody] RestaurantDTO request)
        {
            var restaurant = await _restaurantServices.AddRestaurantAsync(request);
            return Ok(restaurant);
        }

        [HttpPut("edit/{id}")]
        public async Task<IActionResult> EditRestaurantAsync([FromRoute] int id, [FromBody] RestaurantDTO request)
        {
            var restaurant = await _restaurantServices.EditRestaurantAsync(id, request);
            return Ok(restaurant);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteRestaurantAsync([FromRoute] int id)
        {
            var result = await _restaurantServices.DeleteRestaurantAsync(id);
            return Ok(result);
        }

        [HttpGet("by-name")]
        public async Task<IActionResult> GetRestaurantByName([FromQuery] string name)
        {
            var restaurant = await _restaurantServices.GetRestaurantByName(name);
            return Ok(restaurant);
        }

        [HttpGet("by-cuisine")]
        public async Task<IActionResult> GetRestaurantsByCuisineType([FromQuery] string cuisineType)
        {
            var restaurants = await _restaurantServices.GetRestaurantsByCuisineType(cuisineType);
            return Ok(restaurants);
        }

        [HttpGet("{id}/dishes")]
        public async Task<IActionResult> GetDishesForRestaurant([FromRoute] int id)
        {
            var dishes = await _restaurantServices.GetDishesForRestaurant(id);
            return Ok(dishes);
        }

        [HttpGet("{id}/addresses")]
        public async Task<IActionResult> GetAddressesForRestaurant([FromRoute] int id)
        {
            var addresses = await _restaurantServices.GetAddressesForRestaurant(id);
            return Ok(addresses);
        }

        [HttpGet("owner/all")]
        public async Task<IActionResult> GetAllRestaurantsForOwner()
        {
            var restaurants = await _restaurantServices.GetAllRestaurants();
            return Ok(restaurants);
        }
    }
}
