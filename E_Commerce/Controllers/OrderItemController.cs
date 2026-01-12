using Domin.DTOS.DTO;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;

namespace E_Commerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderDishServices _orderItemServices;

        public OrderItemController(IOrderDishServices orderItemServices)
        {
            _orderItemServices = orderItemServices;
        }

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderItemDto orderItem)
        {
            var result = await _orderItemServices.CreateOrder(orderItem);
            return Ok(result);
        }

        [HttpPut("AddDishToExistingOrder/{orderid}")]
        public async Task<IActionResult> AddItemToExistingOrder([FromRoute] int orderid, [FromBody] OrderItemDto orderItem)
        {
            var result = await _orderItemServices.AddDishToExistingOrder(orderid, orderItem);
            return Ok(result);
        }

        [HttpPut("UpdateOrderDishQuantity/{orderItemId}")]
        public async Task<IActionResult> UpdateOrderItemQuantity([FromRoute] int orderItemId, [FromBody] OrderItemDto dto)
        {
            var result = await _orderItemServices.UpdateOrderDishQuantity(orderItemId, dto.Quantity);
            return Ok(result);
        }

        [HttpDelete("DeleteOrderDish/{orderItemId}")]
        public async Task<IActionResult> DeleteOrderItem([FromRoute] int orderItemId)
        {
            var result = await _orderItemServices.DeleteOrderDish(orderItemId);
            return Ok(result);
        }

        [HttpGet("GetAllOrderDishForARestaurant/{restaurantid}")]
        public async Task<IActionResult> GetAllOrderItemsForARestaurant([FromRoute] int restaurantid)
        {
            var result = await _orderItemServices.GetAllOrderDishForARestaurant(restaurantid);
            return Ok(result);
        }

        [HttpGet("GetMostOrderedDishesForARestaurant/{restaurantid}")]
        public async Task<IActionResult> GetMostOrderedItemsForARestaurant([FromRoute] int restaurantid)
        {
            var result = await _orderItemServices.GetMostOrderedDishesForARestaurant(restaurantid);
            return Ok(result);
        }
    }


}
