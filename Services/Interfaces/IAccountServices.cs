using Domin.DTOS.Auth_DTO;
using Domin.Enum;

namespace Repository.Interfaces
{
    public interface IAccountServices
    {
        Task<AuthenticationResponse> Login(AuthenticationRequest request);
        Task<AuthenticationResponse> RefreshToken(RefreshTokenRequest request);
        Task<bool> RevokeToken(string token);
        Task<AuthenticationResponse> RegisterUserAs(Register register, Roles role);
        Task<AuthenticationResponse> LoginWithGoogle(ExternalLoginRequest request, Roles role = Roles.Customer);
        Task ForgotPassword(ForgotPasswordRequest forgotPasswordRequest);
        Task<AuthenticationResponse> ResetPassword(ResetPasswordRequest resetPasswordRequest);
    }
}
