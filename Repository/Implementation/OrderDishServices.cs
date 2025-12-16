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
    public class OrderDishServices : IOrderDishServices

    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpClientaccessor;

        public OrderDishServices(IHttpContextAccessor httpClientaccessor, ApplicationDbContext dbContext)
        {
            _httpClientaccessor = httpClientaccessor;
            _dbContext = dbContext;
        }

        public async Task<OrderItem> AddDishToExistingOrder(int idoforder, OrderItemDto orderItem)
        {
           var order =await _dbContext.Orders.FindAsync(idoforder)?? throw new Exception("Order Not Found");
            var orderitem = new OrderItem
            {
                OrderId = order.OrderId,
                DishId = orderItem.DishId,
                Quantity = orderItem.Quantity,
                PriceAtOrder = orderItem.PriceAtOrder,
                CreatedAt = DateTime.Now
                
            };
            await _dbContext.OrderItems.AddAsync(orderitem);
            await _dbContext.SaveChangesAsync();
            return orderitem;

        }

        public async Task<OrderItem> CreateOrder(OrderItemDto orderItemreq)
        {
            var userid = _httpClientaccessor.HttpContext.User.Claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("USer Not Found") ;
            var dish =  _dbContext.Dishes.SingleOrDefault(o=>o.DishId==orderItemreq.DishId)?? throw new Exception("dish Not Found"); 
            var Address= await _dbContext.Addresses.SingleOrDefaultAsync(o=>o.UserId==userid)?? throw new Exception("Address Not Found"); ;

            var order = new Order
            {
               
                CreatedAt = DateTime.Now,
                Status = OrderStatus.Pending,
                UserId = userid,
                OrderDate = DateTime.Today,
                RestaurantId = dish.RestaurantId,
                AddressId=Address.AddressId,
                
            };

            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();

            var orderitem =new OrderItem
           {
               OrderId = order.OrderId,
               DishId = orderItemreq.DishId,
               Quantity = orderItemreq.Quantity,
               PriceAtOrder = orderItemreq.PriceAtOrder,
               CreatedAt = DateTime.Now
           };

            await _dbContext.OrderItems.AddAsync(orderitem);
            await _dbContext.SaveChangesAsync();


            return orderitem;
        }

        public async Task<string> DeleteOrderDish(int orderItemId)
        {
            var order =await _dbContext.OrderItems.FindAsync(orderItemId)??throw new Exception("OrderItem Not Found");
            _dbContext.OrderItems.Remove(order);
            await _dbContext.SaveChangesAsync();
            return "Deleted Successfuly";
        }

        public async Task<List<OrderItem>> GetAllOrderDishForARestaurant(int restaurantid)
        {
            var ordersitem=await _dbContext.OrderItems.Include(o=>o.Dish).Where(o=>o.Dish !=null &&o.Dish.RestaurantId==restaurantid).ToListAsync();
            if ( !ordersitem.Any())
            {
                throw new Exception("No Order Items Found for this Restaurant");
            }
            return ordersitem;
        }


        public async Task<List<OrderItem>> GetMostOrderedIDishesForARestaurant(int restaurantid)  
        {
            var orderitem=await _dbContext.OrderItems.Include(o => o.Dish)
                .Where(o => o.Dish != null && o.Dish.RestaurantId == restaurantid)
                .GroupBy(o => o.DishId)
                .Select(g => new
                {
                    DishId = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count)
                .Take(5)
                .ToListAsync();
            if(orderitem.Any())
                throw new Exception("No Order Items Found for this Restaurant");
           
            
            return orderitem.Select(o => new OrderItem
            {
                DishId = o.DishId,
                Quantity = o.Count,
                Dish = _dbContext.Dishes.Find(o.DishId) 
            }).ToList();
            

        }

        public async Task<List<Dish>> GetMostPopularDishesAcrossAllRestaurants()
        {
            var dishes = await _dbContext.OrderItems
                .Include(o => o.Dish)
                .Where(o => o.Dish != null)
                .GroupBy(o => o.DishId)
                .Select(g => new
                {
                    DishId = g.Key,
                    Count = g.Count(),
                    Dish = g.First().Dish 
                })
                .OrderByDescending(g => g.Count)
                .Take(5)
                .Select(g => g.Dish)
                .ToListAsync();

            return dishes!;
        }

        public async Task<OrderItem> UpdateOrderDishQuantity(int orderItemId, int Quantity)
        {
            var orderitem = _dbContext.OrderItems.Find(orderItemId) ?? throw new Exception("Order Item Not Found");
            orderitem.Quantity = Quantity;
            orderitem.UpdatedAt = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return orderitem;
        }
    }
}
