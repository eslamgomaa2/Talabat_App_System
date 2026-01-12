using Domin.DTOS.DTO;
using Domin.Models;

namespace Repository.Interfaces
{
    public interface IRestaurantServices
    {

        Task<Restaurant> AddRestaurantAsync(RestaurantDTO request);
        Task<Restaurant> EditRestaurantAsync(int id, RestaurantDTO request);
        Task<Restaurant> DeleteRestaurantAsync(int id);
        Task<List<Restaurant>> GetRestaurantByName(string name);
        Task<List<Restaurant>> GetRestaurantsByCuisineType(string CuisineType);
        Task<List<Dish>> GetDishesForRestaurant(int id);
        Task<List<Address>> GetAddressesForRestaurant(int id);
        Task<List<Restaurant>> GetAllRestaurantsForOwner(int id);
        Task<List<Restaurant>> GetAllRestaurants();


    }
}
