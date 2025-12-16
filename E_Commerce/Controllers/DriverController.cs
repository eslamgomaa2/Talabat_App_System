using Domin.DTOS.DTO;
using Domin.Enum;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;

namespace E_Commerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriverController : ControllerBase
    {
        private readonly IDriverServices _driverServices;

        public DriverController(IDriverServices driverServices)
        {
            _driverServices = driverServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDriversAsync()
        {
            var drivers = await _driverServices.GetAllDriversAsync();
            return Ok(drivers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDriverById(int id)
        {
            var driver = await _driverServices.GetDriverByID(id);
            return Ok(driver);
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetOnlyAvailableDrivers()
        {
            var availableDrivers = await _driverServices.GetOnlyAvailableDrivers();
            return Ok(availableDrivers);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterByVehicleType([FromQuery] Vehicles type)
        {
            var filteredDrivers = await _driverServices.FilterByVehicleType(type);
            return Ok(filteredDrivers);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewDriver([FromBody] DriverDto driver)
        {
            var addedDriver = await _driverServices.AddNewDriver(driver);
            return Ok(addedDriver);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDriver(int id, [FromBody] DriverDto driver)
        {
            var updatedDriver = await _driverServices.UpdateDriver(id, driver);
            return Ok(updatedDriver);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriver(int id)
        {
            var result = await _driverServices.DeleteDriver(id);
            return Ok(result);
        }

        [HttpPatch("{id}/available")]
        public async Task<IActionResult> MarkDriverAsAvailable(int id)
        {
            var driver = await _driverServices.MarkDriverAsAvailable(id);
            return Ok(driver);
        }

        [HttpPatch("{id}/unavailable")]
        public async Task<IActionResult> MarkDriverAsUnAvailable(int id)
        {
            var driver = await _driverServices.MarkDriverAsUnAvailable(id);
            return Ok(driver);
        }

        [HttpGet("{id}/active-orders")]
        public async Task<IActionResult> GetDriverActiveOrders(int id)
        {
            var activeOrders = await _driverServices.GetDriverActiveOrders(id);
            return Ok(activeOrders);
        }

        [HttpGet("{id}/completed-orders")]
        public async Task<IActionResult> GetDriverCompletedOrders(int id)
        {
            
            var completedOrders = await _driverServices.GetDriverCompletedOrders(id);
            return Ok(completedOrders);
        }

        [HttpGet("find-by-phone/{phone}")]
        public async Task<IActionResult> FindDriverByPhone(string phone)
        {
            var driver = await _driverServices.FindDriverByphone(phone);
            return Ok(driver);
        }
    }
}
