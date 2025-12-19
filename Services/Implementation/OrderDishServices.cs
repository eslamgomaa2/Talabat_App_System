using Domin.DTOS.DTO;
using Domin.Enum;
using Domin.Models;
using Microsoft.AspNetCore.Http;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class OrderDishServices : IOrderDishServices
    {
        private readonly IOrderDishRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderDishServices(IOrderDishRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<OrderItem> AddDishToExistingOrder(int orderId, OrderItemDto orderItemDto)
        {
            var order = await _repository.GetOrderByIdAsync(orderId) ?? throw new Exception("Order not found");

            var orderItem = new OrderItem
            {
                OrderId = order.OrderId,
                DishId = orderItemDto.DishId,
                Quantity = orderItemDto.Quantity,
                PriceAtOrder = orderItemDto.PriceAtOrder,
                CreatedAt = DateTime.Now
            };

            await _repository.AddOrderItemAsync(orderItem);
            return orderItem;
        }

        public async Task<OrderItem> CreateOrder(OrderItemDto orderItemDto)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.Claims
                         ?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
                         ?? throw new Exception("User not found");

            var dish = await _repository.GetDishByIdAsync(orderItemDto.DishId) ?? throw new Exception("Dish not found");
            var address = await _repository.GetAddressByUserIdAsync(userId) ?? throw new Exception("Address not found");

            var order = new Order
            {
                CreatedAt = DateTime.Now,
                Status = OrderStatus.Pending,
                UserId = userId,
                OrderDate = DateTime.Today,
                RestaurantId = dish.RestaurantId,
                AddressId = address.AddressId
            };

            await _repository.AddOrderAsync(order);

            var orderItem = new OrderItem
            {
                OrderId = order.OrderId,
                DishId = orderItemDto.DishId,
                Quantity = orderItemDto.Quantity,
                PriceAtOrder = orderItemDto.PriceAtOrder,
                CreatedAt = DateTime.Now
            };

            await _repository.AddOrderItemAsync(orderItem);
            return orderItem;
        }

        public async Task<string> DeleteOrderDish(int orderItemId)
        {
            var orderItem = await _repository.GetOrderItemByIdAsync(orderItemId) ?? throw new Exception("Order item not found");
            await _repository.DeleteOrderItemAsync(orderItem);
            return "Deleted successfully";
        }

        public async Task<List<OrderItem>> GetAllOrderDishForARestaurant(int restaurantId)
        {
            var items = await _repository.GetAllOrderItemsForRestaurantAsync(restaurantId);
            if (!items.Any()) throw new Exception("No order items found for this restaurant");
            return items;
        }

        public async Task<List<OrderItem>> GetMostOrderedDishesForARestaurant(int restaurantId)
        {
            var items = await _repository.GetMostOrderedDishesForRestaurantAsync(restaurantId);
            if (!items.Any()) throw new Exception("No order items found for this restaurant");
            return items;
        }

        public async Task<List<Dish>> GetMostPopularDishesAcrossAllRestaurants()
        {
            return await _repository.GetMostPopularDishesAcrossAllRestaurantsAsync();
        }

        public async Task<OrderItem> UpdateOrderDishQuantity(int orderItemId, int quantity)
        {
            var orderItem = await _repository.GetOrderItemByIdAsync(orderItemId) ?? throw new Exception("Order item not found");
            orderItem.Quantity = quantity;
            orderItem.UpdatedAt = DateTime.Now;
            await _repository.UpdateOrderItemAsync(orderItem);
            return orderItem;
        }
    }
}
