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
    public class OrderServices : IOrderServices
    {
        private readonly IOrderRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderServices(IOrderRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Order> CancelOrder(int orderId)
        {
            var order = await _repository.GetByIdAsync(orderId) ?? throw new Exception("No order found with the given ID.");
            await _repository.DeleteAsync(order);
            return order;
        }

        public async Task<List<Order>> GetAllOrders()
        {
            var orders = await _repository.GetAllAsync();
            if (!orders.Any()) throw new Exception("No orders found.");
            return orders;
        }

        public async Task<List<Order>> GetOrdersBySpecificStatus(OrderStatus status)
        {
            var orders = await _repository.GetByStatusAsync(status);
            if (!orders.Any()) throw new KeyNotFoundException("No orders found with the specified status.");
            return orders;
        }

        public async Task<List<Order>> GetOrdersDetailsByID(int id)
        {
            var orders = await _repository.GetByIdAsync(id) != null
                         ? new List<Order> { await _repository.GetByIdAsync(id) }
                         : new List<Order>();

            if (!orders.Any()) throw new KeyNotFoundException("No order found with the given ID.");
            return orders;
        }

        public async Task<List<Order>> GetOrdersForSpecificRestaurant(int restaurantId)
        {
            var orders = await _repository.GetByRestaurantIdAsync(restaurantId);
            if (!orders.Any()) throw new KeyNotFoundException("No orders found for the specified restaurant.");
            return orders;
        }

        public async Task<List<Order>> GetPendingOrdersForRestaurant(int restaurantId)
        {
            var orders = await _repository.GetPendingOrdersByRestaurantIdAsync(restaurantId);
            if (!orders.Any()) throw new KeyNotFoundException("No pending orders found for the specified restaurant.");
            return orders;
        }

        public async Task<List<Order>> GetUsersCurrentActiveOrders(string userId)
        {
            var orders = await _repository.GetActiveOrdersByUserIdAsync(userId);
            if (!orders.Any()) throw new KeyNotFoundException("No active orders found for the user.");
            return orders;
        }

        public async Task<List<Order>> GetUsersOrderHistory(string userId)
        {
            var orders = await _repository.GetByUserIdAsync(userId);
            if (!orders.Any()) throw new KeyNotFoundException("No orders found for the specified user.");
            return orders;
        }

        public async Task<Order> UpdateOrder(int id, OrderDto orderReq)
        {
            var order = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException("No order found with the given ID.");

            order.OrderDate = orderReq.OrderDate;
            order.TotalAmount = orderReq.TotalAmount;
            order.AddressId = orderReq.DeliveryAddressId;
            order.UpdatedAt = DateTime.UtcNow;
            order.Status = orderReq.Status;

            await _repository.UpdateAsync(order);
            return order;
        }

        public async Task<Order> UpdateOrderStatus(int id, OrderStatus status)
        {
            var order = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException("No order found with the given ID.");
            order.Status = status;
            order.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(order);
            return order;
        }
    }
}
