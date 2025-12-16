using Domin.DTOS.Auth_DTO;
using Domin.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IAccountServices
    {
        Task<AuthenticationResponse> Login(AuthenticationRequest request);
        Task<AuthenticationResponse> RegisterUserAs(Register register, Roles role);
        Task ForgotPassword(ForgotPasswordRequest forgotPasswordRequest);
        Task<AuthenticationResponse> ResetPassword(ResetPasswordRequest resetPasswordRequest);
    }
}
