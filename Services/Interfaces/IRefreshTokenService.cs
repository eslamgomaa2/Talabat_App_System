using Domin.DTOS.Auth_DTO;
using Domin.Models;

namespace Repository.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<RefreshToken> CreateRefreshTokenAsync(int userId);
        Task<RefreshToken?> ValidateRefreshTokenAsync(string token);
        Task RevokeRefreshTokenAsync(string token, string? replacedByToken = null);
        Task RevokeAllUserTokensAsync(int userId);
        Task CleanupExpiredTokensAsync();
    }
}