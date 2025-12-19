using Domin.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class OrderDishRepository : IOrderDishRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderDishRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddOrderAsync(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddOrderItemAsync(OrderItem orderItem)
        {
            await _dbContext.OrderItems.AddAsync(orderItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteOrderItemAsync(OrderItem orderItem)
        {
            _dbContext.OrderItems.Remove(orderItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Dish> GetDishByIdAsync(int dishId)
        {
            return await _dbContext.Dishes.FindAsync(dishId);
        }

        public async Task<Address> GetAddressByUserIdAsync(string userId)
        {
            return await _dbContext.Addresses.SingleOrDefaultAsync(a => a.UserId == userId);
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _dbContext.Orders.FindAsync(orderId);
        }

        public async Task<OrderItem> GetOrderItemByIdAsync(int id)
        {
            return await _dbContext.OrderItems.FindAsync(id);
        }

        public async Task<List<OrderItem>> GetAllOrderItemsForRestaurantAsync(int restaurantId)
        {
            return await _dbContext.OrderItems.Include(o => o.Dish)
                .Where(o => o.Dish != null && o.Dish.RestaurantId == restaurantId)
                .ToListAsync();
        }

        public async Task<List<OrderItem>> GetMostOrderedDishesForRestaurantAsync(int restaurantId)
        {
            var items = await _dbContext.OrderItems.Include(o => o.Dish)
                .Where(o => o.Dish != null && o.Dish.RestaurantId == restaurantId)
                .GroupBy(o => o.DishId)
                .Select(g => new { DishId = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Take(5)
                .ToListAsync();

            return items.Select(o => new OrderItem
            {
                DishId = o.DishId,
                Quantity = o.Count,
                Dish = _dbContext.Dishes.Find(o.DishId)
            }).ToList();
        }

        public async Task<List<Dish>> GetMostPopularDishesAcrossAllRestaurantsAsync()
        {
            return await _dbContext.OrderItems
                .Include(o => o.Dish)
                .Where(o => o.Dish != null)
                .GroupBy(o => o.DishId)
                .Select(g => g.First().Dish)
                .OrderByDescending(d => _dbContext.OrderItems.Count(o => o.DishId == d.DishId))
                .Take(5)
                .ToListAsync();
        }

        public async Task UpdateOrderItemAsync(OrderItem orderItem)
        {
            _dbContext.OrderItems.Update(orderItem);
            await _dbContext.SaveChangesAsync();
        }
    }
}
