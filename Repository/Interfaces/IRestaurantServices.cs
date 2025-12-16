using Domin.DTOS.DTO;
using Domin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IRestaurantServices
    {
        
        Task<Restaurant> AddRestaurantAsync(RestaurantDTO request);
        Task<Restaurant> EditRestaurantAsync(int id,RestaurantDTO request);
        Task<Restaurant> DeletRestaurantAsync(int id);
        Task<List<Restaurant>> GetRestaurantByName(string name);
        Task<List<Restaurant>> GetRestaurantsByCuisineType(string CuisineType);
        Task<List<Dish>> GetDishesforRestaurant(int id);
        Task<List<Address>> GetAddressesforRestaurant(int id);
        Task<List<Restaurant>> GetAllRestaurantsforOwner(int id);
        Task<List<Restaurant>> GetAllRestaurants();

        
    }
}
