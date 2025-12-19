using Domin.DTOS.DTO;
using Domin.Enum;
using Domin.Models;
using MailKit.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IOrderServices
    {
        Task<List<Order>> GetAllOrders();
        Task<List<Order>> GetOrdersDetailsByID(int id);
        Task<Order> UpdateOrder(int id,OrderDto order);
        Task<Order> CancelOrder(int id);
        Task<Order> UpdateOrderStatus(int id,OrderStatus status);
        Task<List<Order>> GetOrdersBySpecificStatus(OrderStatus status);
        Task<List<Order>> GetUsersOrderHistory(string id);
        Task<List<Order>> GetPendingOrdersForRestaurant(int id);


    }
}
