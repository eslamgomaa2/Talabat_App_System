using Domin.DTOS.DTO;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;

namespace E_Commerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressServices _addressservice;

        public AddressController(IAddressServices addressservice)
        {
            _addressservice = addressservice;
        }

        [HttpGet("GetUserAddress/{userid}")]
        public async Task<IActionResult> GetUserAddress([FromRoute] string userid)
        {
            var res = await _addressservice.GetUserAddressAsync(userid);
            return Ok(res);
        }

        [HttpPost("SetUserAddress")]
        public async Task<IActionResult> SetUserAddress([FromBody] AddressDTO request)
        {
            var res = await _addressservice.SetUserAddress(request);
            return Ok(res);
        }

        [HttpPut("EditUserAddress/{userid}")]
        public async Task<IActionResult> EditUserAddress([FromBody] AddressDTO request, [FromRoute] string userid)
        {
            var res = await _addressservice.EditUserAddressAsync(userid, request);
            return Ok(res);
        }

        [HttpDelete("DeleteUserAddress/{userid}")]
        public async Task<IActionResult> DeleteUserAddress([FromRoute] string userid)
        {
            var res = await _addressservice.DeleteUserAddressAsync(userid);
            return Ok(res);
        }
    }
}
