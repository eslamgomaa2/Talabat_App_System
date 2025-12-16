using Domin.DTOS.DTO;
using Domin.Models;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;

namespace E_Commerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfile _userprofile;

        public UserProfileController(IUserProfile userprofile)
        {
            this._userprofile = userprofile;
        }

        [HttpPut("EditUser/{userid}")]
        public async Task<IActionResult> EditUser([FromBody] UserDto userrequest,[FromRoute] string userid)
        {
            var res = await _userprofile.EditUser(userid, userrequest);

            return Ok(res);
        }

       
    }
}
