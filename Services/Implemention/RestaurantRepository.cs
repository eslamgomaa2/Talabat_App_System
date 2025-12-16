using Domin.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RestaurantRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Restaurant> AddRestaurantAsync(Restaurant restaurant)
        {
            _dbContext.Restaurants.Add(restaurant);
            await _dbContext.SaveChangesAsync();
            return restaurant;
        }

        public async Task<Restaurant> DeleteRestaurantAsync(int id)
        {
            var restaurant = await _dbContext.Restaurants.FindAsync(id) ?? throw new KeyNotFoundException("Restaurant not found");
            _dbContext.Restaurants.Remove(restaurant);
            await _dbContext.SaveChangesAsync();
            return restaurant;
        }

        public async Task<Restaurant> EditRestaurantAsync(Restaurant restaurant)
        {
            _dbContext.Restaurants.Update(restaurant);
            await _dbContext.SaveChangesAsync();
            return restaurant;
        }

        public async Task<Restaurant> GetRestaurantByIdAsync(int id)
        {
            var restaurant = await _dbContext.Restaurants
                .Include(r => r.Dishes)
                .SingleOrDefaultAsync(r => r.RestaurantId == id)
                ?? throw new KeyNotFoundException("Restaurant not found");
            return restaurant;
        }

        public async Task<List<Restaurant>> GetAllRestaurantsAsync()
        {
            var restaurants = await _dbContext.Restaurants.ToListAsync();
            if (!restaurants.Any())
                throw new KeyNotFoundException("No restaurants found");
            return restaurants;
        }

        public async Task<List<Restaurant>> GetRestaurantsByCuisineTypeAsync(string cuisineType)
        {
            var restaurants = await _dbContext.Restaurants
                .Where(r => r.CuisineType == cuisineType)
                .ToListAsync();
            if (!restaurants.Any())
                throw new KeyNotFoundException("No restaurants found for this cuisine type");
            return restaurants;
        }

        public async Task<List<Restaurant>> GetRestaurantsByNameAsync(string name)
        {
            var restaurants = await _dbContext.Restaurants
                .Where(r => r.Name == name)
                .ToListAsync();
            if (!restaurants.Any())
                throw new KeyNotFoundException("No restaurants found with this name");
            return restaurants;
        }

        public async Task<List<Restaurant>> GetAllRestaurantsForOwnerAsync(int ownerId)
        {
            var restaurants = await _dbContext.Restaurants
                .Where(r => r.OwnerId == ownerId)
                .ToListAsync();
            if (!restaurants.Any())
                throw new KeyNotFoundException("No restaurants found for this owner");
            return restaurants;
        }

        public async Task<List<Address>> GetAddressesForRestaurantAsync(int restaurantId)
        {
            var addresses = await _dbContext.Addresses
                .Where(a => a.RestaurantId == restaurantId)
                .ToListAsync();
            if (!addresses.Any())
                throw new KeyNotFoundException("No addresses found for this restaurant");
            return addresses;
        }

        public async Task<List<Dish>> GetDishesForRestaurantAsync(int restaurantId)
        {
            var restaurant = await _dbContext.Restaurants
                .Include(r => r.Dishes)
                .SingleOrDefaultAsync(r => r.RestaurantId == restaurantId)
                ?? throw new KeyNotFoundException("Restaurant not found");

            return restaurant.Dishes?.ToList() ?? new List<Dish>();
        }
    }
}
