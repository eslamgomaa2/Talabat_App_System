using Domin.DTOS.DTO;
using Domin.Enum;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;

namespace E_Commerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryDetailsController : ControllerBase
    {
        private readonly IDeliveryDetailsServices _deliveryDetailsServices;

        public DeliveryDetailsController(IDeliveryDetailsServices deliveryDetailsServices)
        {
            _deliveryDetailsServices = deliveryDetailsServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDeliveryDetailsAsync()
        {
            var deliveryDetails = await _deliveryDetailsServices.GetAllDeliveryDetailsAsync();
            return Ok(deliveryDetails);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeliveryByID(int id)
        {
            var deliveryDetail = await _deliveryDetailsServices.GetDeliveryByID(id);
            return Ok(deliveryDetail);
        }

        [HttpGet("by-order/{orderId}")]
        public async Task<IActionResult> GetDeliveryByOrderID(int orderId)
        {
            var deliveryDetail = await _deliveryDetailsServices.GetDeliveryByOrderID(orderId);
            return Ok(deliveryDetail);
        }

        [HttpGet("by-driver/{driverId}")]
        public async Task<IActionResult> GetAllDeliveryForADriver(int driverId)
        {
            var deliveryDetails = await _deliveryDetailsServices.GetAllDeliveryForADriver(driverId);
            return Ok(deliveryDetails);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewDelivery([FromBody] DeliveryDetailsDto request)
        {
            var newDelivery = await _deliveryDetailsServices.CreateNewDelivery(request);
            return Ok(newDelivery);
        }

        [HttpPut("UpdateEntireDeliveryRecord/{deliveryId}")]
        public async Task<IActionResult> UpdateEntireDeliveryRecord(int deliveryId, [FromBody] DeliveryDetailsDto request)
        {
            var updatedDelivery = await _deliveryDetailsServices.UpdateEntireDeliveryRecord(deliveryId, request);
            return Ok(updatedDelivery);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateDeliveryStatus(int id, [FromQuery] DeliveryStatus status)
        {
            var updatedStatus = await _deliveryDetailsServices.UpdateDeliveryStatus(id, status);
            return Ok(updatedStatus);
        }

        [HttpPatch("{deliveryId}/mark-delivered")]
        public async Task<IActionResult> MarkAsDeliveredt_setDeliveredTime(int deliveryId)
        {
            var updatedDelivery = await _deliveryDetailsServices.MarkAsDeliveredt_setDeliveredTime(deliveryId);
            return Ok(updatedDelivery);
        }

        [HttpPatch("{deliveryId}/cancel")]
        public async Task<IActionResult> Canceldelivery(int deliveryId)
        {
            var canceledDelivery = await _deliveryDetailsServices.Canceldelivery(deliveryId);
            return Ok(canceledDelivery);
        }

        [HttpGet("{deliveryId}/status")]
        public async Task<IActionResult> GetCurrentDeliveryStatus(int deliveryId)
        {
            var status = await _deliveryDetailsServices.GetCurrentDeliveryStatus(deliveryId);
            return Ok(status);
        }

        [HttpGet("filter-by-status")]
        public async Task<IActionResult> GetDeliveryDetailsByStatus([FromQuery] DeliveryStatus status)
        {
            var deliveryDetails = await _deliveryDetailsServices.GetDeliveryDetailsByStatus(status);
            return Ok(deliveryDetails);
        }

        [HttpGet("driver-history/{driverId}")]
        public async Task<IActionResult> DriversDeliveryHistory(int driverId)
        {
            var deliveryHistory = await _deliveryDetailsServices.DriversDeliveryHistory(driverId);
            return Ok(deliveryHistory);
        }
    }
}
