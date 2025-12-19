using Domin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IOrderDishRepository
    {
        Task<OrderItem> GetOrderItemByIdAsync(int id);
        Task<List<OrderItem>> GetAllOrderItemsForRestaurantAsync(int restaurantId);
        Task<List<OrderItem>> GetMostOrderedDishesForRestaurantAsync(int restaurantId);
        Task<List<Dish>> GetMostPopularDishesAcrossAllRestaurantsAsync();
        Task AddOrderAsync(Order order);
        Task AddOrderItemAsync(OrderItem orderItem);
        Task UpdateOrderItemAsync(OrderItem orderItem);
        Task DeleteOrderItemAsync(OrderItem orderItem);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<Dish> GetDishByIdAsync(int dishId);
        Task<Address> GetAddressByUserIdAsync(string userId);
    }
}
