using Domin.DTOS.DTO;
using Domin.Enum;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;

namespace E_Commerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _orderServices;

        public OrderController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        [HttpGet("GetAllOrders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderServices.GetAllOrders();
            return Ok(orders);
        }

        [HttpGet("GetOrdersDetailsByID/{id}")]
        public async Task<IActionResult> GetOrdersDetailsByID([FromRoute] int id)
        {
            var orderDetails = await _orderServices.GetOrdersDetailsByID(id);
            return Ok(orderDetails);
        }

        // ❌ Invalid: [FromBody] in GET – changed to PUT and removed model state check for GET
        [HttpPut("UpdateOrder/{id}")]
        public async Task<IActionResult> UpdateOrder([FromRoute] int id, [FromBody] OrderDto order)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updatedOrder = await _orderServices.UpdateOrder(id, order);
            return Ok(updatedOrder);
        }

        [HttpPut("CancelOrder/{id}")]
        public async Task<IActionResult> CancelOrder([FromRoute] int id)
        {
            var cancelledOrder = await _orderServices.CancelOrder(id);
            return Ok(cancelledOrder);
        }

        [HttpPut("UpdateOrderStatus/{id}")]
        public async Task<IActionResult> UpdateOrderStatus([FromRoute] int id, [FromQuery] OrderStatus status)
        {
            var updatedStatus = await _orderServices.UpdateOrderStatus(id, status);
            return Ok(updatedStatus);
        }

        [HttpGet("GetOrdersBySpecificStatus")]
        public async Task<IActionResult> GetOrdersBySpecificStatus([FromQuery] OrderStatus status)
        {
            var ordersByStatus = await _orderServices.GetOrdersBySpecificStatus(status);
            return Ok(ordersByStatus);
        }

        [HttpGet("GetUsersOrderHistory/{id}")]
        public async Task<IActionResult> GetUsersOrderHistory([FromRoute] string id)
        {
            var userOrderHistory = await _orderServices.GetUsersOrderHistory(id);
            return Ok(userOrderHistory);
        }

        [HttpGet("GetPendingOrdersForRestaurant/{id}")]
        public async Task<IActionResult> GetPendingOrdersForRestaurant([FromRoute] int id)
        {
            var res = await _orderServices.GetPendingOrdersForRestaurant(id);
            return Ok(res);
        }
    }
}
