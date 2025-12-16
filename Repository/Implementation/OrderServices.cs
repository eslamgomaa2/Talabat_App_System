using Domin.DTOS.DTO;
using Domin.Enum;
using Domin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class OrderServices : IOrderServices
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderServices(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbcontext)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbcontext = dbcontext;
        }

       

        public async Task<Order> CancelOrder(int orderid)
        {
            var order = await _dbcontext.Orders.FindAsync(orderid)?? throw new Exception(" No order found with the given ID."); 
            _dbcontext.Orders.Remove(order);
            await _dbcontext.SaveChangesAsync();
            return order;
        }

        public async Task<List<Order>> GetAllOrders()
        {
            var Orders = await _dbcontext.Orders.ToListAsync();
            if (!Orders.Any())
                throw new Exception("No orders found .");
            return Orders;
        }



        public async Task<List<Order>> GetOrdersBySpecificStatus(OrderStatus status)
        {
            var orders = await _dbcontext.Orders
                .Where(o => o.Status == status)
                .ToListAsync();

            if (!orders.Any())
            {
                throw new KeyNotFoundException("No orders found with the specified status.");
            }

            return orders;
        }

        public async Task<List<Order>> GetOrdersDetailsByID(int id)
        {
           var orders=await _dbcontext.Orders.Where(o=>o.OrderId==id).ToListAsync();
            if (!orders.Any())
                throw new KeyNotFoundException("No order found with the given ID.");
            return orders;
        }

        public async Task<List<Order>> GetOrdersForSpecificRestaurant(int id)
        {
           var orders=await _dbcontext.Orders.Where(o=>o.RestaurantId==id).ToListAsync();
            if (!orders.Any())
                throw new KeyNotFoundException("No orders found for the specified restaurant.");
            return orders;
        }

        public async Task<List<Order>> GetPendingOrdersForRestaurant(int restaurantid)
        {
          var orders=await _dbcontext.Orders
                .Where(o => o.RestaurantId == restaurantid && o.Status == OrderStatus.Pending)
                .ToListAsync();
            if (!orders.Any())
                throw new KeyNotFoundException("No pending orders found for the specified restaurant.");
            return orders;
        }

        public async Task<List<Order>> GetUsersCurrentActiveOrders(string id)
        {
            

            var orders = await _dbcontext.Orders
                .Where(o => o.UserId == id && o.Status==OrderStatus.Preparing)
                .Include(o => o.OrderItem)!
                .ThenInclude(oi => oi.Dish) 
                .ToListAsync();
            if (!orders.Any())
                throw new KeyNotFoundException("No pending orders found for the specified restaurant.");
            return orders;
           
        }


        public async Task<List<Order>> GetUsersOrderHistory(string id)
        {
          var orders=await _dbcontext.Orders.Include(o=>o.OrderItem)!.ThenInclude(oi=>oi.Dish).Where(o=>o.UserId==id).ToListAsync();
            if (!orders.Any())
                throw new KeyNotFoundException("No orders found for the specified user.");
            return orders;
        }

        public async Task<Order> UpdateOrder(int id, OrderDto orderreq)
        {
            var order = await _dbcontext.Orders
                .SingleOrDefaultAsync(o => o.OrderId == id)
                ?? throw new KeyNotFoundException("No orders found for the given Id.");

            
            order.OrderDate = orderreq.OrderDate;
            order.TotalAmount = orderreq.TotalAmount;
            order.AddressId = orderreq.DeliveryAddressId;
            order.UpdatedAt = DateTime.UtcNow;
            order.Status = orderreq.Status;

            await _dbcontext.SaveChangesAsync();
            return order;
        }


        public async Task<Order> UpdateOrderStatus(int id, OrderStatus status)
        {
          var order = await _dbcontext.Orders.SingleOrDefaultAsync(o => o.OrderId == id) ?? throw new KeyNotFoundException("No order found with the given ID.");
            order.Status = status;
            order.UpdatedAt = DateTime.Now;
            _dbcontext.Orders.Update(order);
            await _dbcontext.SaveChangesAsync();
            return order;
        }
    }
}
