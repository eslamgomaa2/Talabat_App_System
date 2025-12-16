using Domin.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class DishRepository : IDishRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DishRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Dish> GetByIdAsync(int id)
        {
            return await _dbContext.Dishes.Include(d => d.Restaurant)
                                         .SingleOrDefaultAsync(d => d.DishId == id);
        }

        public async Task<List<Dish>> GetAllAsync()
        {
            return await _dbContext.Dishes.ToListAsync();
        }

        public async Task<List<Dish>> GetByNameAsync(string name)
        {
            return await _dbContext.Dishes.Include(d => d.Restaurant)
                                         .Where(d => d.Name == name)
                                         .ToListAsync();
        }

        public async Task<List<Dish>> GetAvailableForRestaurantAsync(int restaurantId)
        {
            return await _dbContext.Dishes.Include(d => d.Restaurant)
                                         .Where(d => d.RestaurantId == restaurantId && d.IsAvailable)
                                         .ToListAsync();
        }

        public async Task AddAsync(Dish dish)
        {
            await _dbContext.Dishes.AddAsync(dish);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Dish dish)
        {
            _dbContext.Dishes.Update(dish);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Dish dish)
        {
            _dbContext.Dishes.Remove(dish);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> RestaurantExistsAsync(int restaurantId)
        {
            return await _dbContext.Restaurants.AnyAsync(r => r.RestaurantId == restaurantId);
        }
    }
}
