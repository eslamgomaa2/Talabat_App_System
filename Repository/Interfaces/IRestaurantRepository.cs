using Domin.DTOS.DTO;
using Domin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IRestaurantRepository
    {
        Task<Restaurant> AddRestaurantAsync(Restaurant restaurant);
        Task<Restaurant> DeleteRestaurantAsync(int id);
        Task<Restaurant> EditRestaurantAsync(Restaurant restaurant);
        Task<Restaurant> GetRestaurantByIdAsync(int id);
        Task<List<Restaurant>> GetAllRestaurantsAsync();
        Task<List<Restaurant>> GetRestaurantsByCuisineTypeAsync(string cuisineType);
        Task<List<Restaurant>> GetRestaurantsByNameAsync(string name);
        Task<List<Restaurant>> GetAllRestaurantsForOwnerAsync(int ownerId);
        Task<List<Address>> GetAddressesForRestaurantAsync(int restaurantId);
        Task<List<Dish>> GetDishesForRestaurantAsync(int restaurantId);
    }
}
