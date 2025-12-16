using Domin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IDishRepository
    {
        Task<Dish> GetByIdAsync(int id);
        Task<List<Dish>> GetAllAsync();
        Task<List<Dish>> GetByNameAsync(string name);
        Task<List<Dish>> GetAvailableForRestaurantAsync(int restaurantId);
        Task AddAsync(Dish dish);
        Task UpdateAsync(Dish dish);
        Task DeleteAsync(Dish dish);
        Task<bool> RestaurantExistsAsync(int restaurantId);
    }
}
