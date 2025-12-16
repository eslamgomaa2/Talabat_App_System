
using Domin.DTOS.Auth_DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;
using ForgotPasswordRequest = Domin.DTOS.Auth_DTO.ForgotPasswordRequest;
using ResetPasswordRequest = Domin.DTOS.Auth_DTO.ResetPasswordRequest;

namespace E_Commerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class AccountController : ControllerBase
    {
        private readonly IAccountServices _accountServices;

        public AccountController(IAccountServices accountServices)
        {
            _accountServices = accountServices;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> login([FromBody]AuthenticationRequest request ) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _accountServices.Login(request);
            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        
        }

        [HttpPost("RegisterUserAscustomer")]
        public async Task<IActionResult> Register([FromBody] Register request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _accountServices.RegisterUserAscustomer(request);
            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }

            return Ok(new{ result.Message,result.IsAuthenticated});

        }
        [HttpPost("RegisterUserAsDriver")]
        public async Task<IActionResult> RegisterUserADriver([FromBody] Register request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _accountServices.RegisterUserAsDriver(request);
            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }

            return Ok(new { result.Message, result.IsAuthenticated });

        }
        [HttpPost("RegisterUserAsRestaurantOwner")]
        public async Task<IActionResult> RegisterUserAsRestaurantOwner([FromBody] Register request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _accountServices.RegisterUserAsRestaurantOwner(request);
            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }

            return Ok(new { result.Message, result.IsAuthenticated });

        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> Forgetpassword([FromBody] ForgotPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _accountServices.ForgotPassword(request);
            return Ok("Check your email for reset password link");

        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> Resetpassword([FromBody] ResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           var res= await _accountServices.ResetPassword(request);
            return Ok(res);

        }
    }
}


