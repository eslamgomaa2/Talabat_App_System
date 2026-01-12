using Domin.DTOS.DTO;
using Domin.Models;

namespace Repository.Interfaces
{
    public interface IOrderDishServices
    {
        Task<OrderItem> CreateOrder(OrderItemDto orderItem);
        Task<OrderItem> AddDishToExistingOrder(int orderid, OrderItemDto orderItem);
        Task<OrderItem> UpdateOrderDishQuantity(int orderItemId, int Quantity);
        Task<string> DeleteOrderDish(int orderItemId);
        Task<List<OrderItem>> GetAllOrderDishForARestaurant(int restaurantid);
        Task<List<OrderItem>> GetMostOrderedDishesForARestaurant(int restaurantid);
        Task<List<Dish>> GetMostPopularDishesAcrossAllRestaurants();

    }

}
