using Domin.DTOS.Auth_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IAccountServices
    {
        public Task<AuthenticationResponse> Login(AuthenticationRequest request);
        public Task<AuthenticationResponse> RegisterUserAscustomer(Register register);
        public Task<AuthenticationResponse> RegisterUserAsDriver(Register register);
        public Task<AuthenticationResponse> RegisterUserAsRestaurantOwner(Register register);
        public Task ForgotPassword(ForgotPasswordRequest forgotPasswordRequest);
        public Task<AuthenticationResponse> ResetPassword(ResetPasswordRequest resetPasswordRequest);
    }
}
