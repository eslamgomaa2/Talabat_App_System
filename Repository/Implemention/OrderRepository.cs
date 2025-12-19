using Domin.Enum;
using Domin.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Order order)
        {
            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _dbContext.Orders.ToListAsync();
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _dbContext.Orders.FindAsync(id);
        }

        public async Task<List<Order>> GetByStatusAsync(OrderStatus status)
        {
            return await _dbContext.Orders.Where(o => o.Status == status).ToListAsync();
        }

        public async Task<List<Order>> GetByUserIdAsync(string userId)
        {
            return await _dbContext.Orders
                .Include(o => o.OrderItem)
                .ThenInclude(oi => oi.Dish)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Order>> GetActiveOrdersByUserIdAsync(string userId)
        {
            return await _dbContext.Orders
                .Include(o => o.OrderItem)
                .ThenInclude(oi => oi.Dish)
                .Where(o => o.UserId == userId && o.Status == OrderStatus.Preparing)
                .ToListAsync();
        }

        public async Task<List<Order>> GetByRestaurantIdAsync(int restaurantId)
        {
            return await _dbContext.Orders.Where(o => o.RestaurantId == restaurantId).ToListAsync();
        }

        public async Task<List<Order>> GetPendingOrdersByRestaurantIdAsync(int restaurantId)
        {
            return await _dbContext.Orders
                .Where(o => o.RestaurantId == restaurantId && o.Status == OrderStatus.Pending)
                .ToListAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            _dbContext.Orders.Update(order);
            await _dbContext.SaveChangesAsync();
        }
    }
}
