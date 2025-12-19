using Domin.Enum;
using Domin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(int id);
        Task<List<Order>> GetAllAsync();
        Task<List<Order>> GetByStatusAsync(OrderStatus status);
        Task<List<Order>> GetByUserIdAsync(string userId);
        Task<List<Order>> GetActiveOrdersByUserIdAsync(string userId);
        Task<List<Order>> GetByRestaurantIdAsync(int restaurantId);
        Task<List<Order>> GetPendingOrdersByRestaurantIdAsync(int restaurantId);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Order order);
    }
}
