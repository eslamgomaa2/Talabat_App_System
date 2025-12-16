using Domin.DTOS.DTO;
using Domin.Models;
using Microsoft.AspNetCore.Http;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class RestaurantServices : IRestaurantServices
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RestaurantServices(IRestaurantRepository restaurantRepository, IHttpContextAccessor httpContextAccessor)
        {
            _restaurantRepository = restaurantRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Restaurant> AddRestaurantAsync(RestaurantDTO request)
        {
            var ownerIdClaim = _httpContextAccessor.HttpContext?.User?.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (ownerIdClaim is null)
                throw new Exception("Problem retrieving user ID");

            var restaurant = MapToRestaurant(request);
            restaurant.OwnerId = int.Parse(ownerIdClaim); // Assuming owner ID is int
            return await _restaurantRepository.AddRestaurantAsync(restaurant);
        }

        public Task<Restaurant> DeleteRestaurantAsync(int id) => _restaurantRepository.DeleteRestaurantAsync(id);

        public async Task<Restaurant> EditRestaurantAsync(int id, RestaurantDTO request)
        {
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(id);
            restaurant.Name = request.Name;
            restaurant.Description = request.Description;
            restaurant.CuisineType = request.CuisineType;
            restaurant.ContactPhone = request.ContactPhone;
            restaurant.OpeningTime = request.OpeningTime;
            restaurant.ClosingTime = request.ClosingTime;
            restaurant.IsActive = request.IsActive;
            restaurant.UpdatedAt = DateTime.Now;

            return await _restaurantRepository.EditRestaurantAsync(restaurant);
        }

        public Task<List<Restaurant>> GetAllRestaurants() => _restaurantRepository.GetAllRestaurantsAsync();

        public Task<List<Restaurant>> GetRestaurantsByCuisineType(string cuisineType)
            => _restaurantRepository.GetRestaurantsByCuisineTypeAsync(cuisineType);

        public Task<List<Restaurant>> GetRestaurantByName(string name)
            => _restaurantRepository.GetRestaurantsByNameAsync(name);

        public Task<List<Restaurant>> GetAllRestaurantsForOwner(int ownerId)
            => _restaurantRepository.GetAllRestaurantsForOwnerAsync(ownerId);

        public Task<List<Address>> GetAddressesForRestaurant(int restaurantId)
            => _restaurantRepository.GetAddressesForRestaurantAsync(restaurantId);

        public Task<List<Dish>> GetDishesForRestaurant(int restaurantId)
            => _restaurantRepository.GetDishesForRestaurantAsync(restaurantId);

        private Restaurant MapToRestaurant(RestaurantDTO request)
        {
            return new Restaurant
            {
                Name = request.Name,
                Description = request.Description,
                CuisineType = request.CuisineType,
                ContactPhone = request.ContactPhone,
                OpeningTime = request.OpeningTime,
                ClosingTime = request.ClosingTime,
                IsActive = request.IsActive,
                CreatedAt = DateTime.Now
            };
        }
    }
}
