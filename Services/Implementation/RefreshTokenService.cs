using Domin.Models;
using Repository.Interfaces;

namespace Repository.Implementation
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _repository;

        public RefreshTokenService(IRefreshTokenRepository repository)
        {
            _repository = repository;
        }

        public async Task<RefreshToken> CreateRefreshTokenAsync(int userId)
        {
            return await _repository.CreateAsync(userId);
        }

        public async Task<RefreshToken?> ValidateRefreshTokenAsync(string token)
        {
            return await _repository.GetValidTokenAsync(token);
        }

        public async Task RevokeRefreshTokenAsync(string token, string? replacedByToken = null)
        {
            var refreshToken = await _repository.GetValidTokenAsync(token);
            if (refreshToken != null)
            {
                refreshToken.IsRevoked = true;
                refreshToken.RevokedAt = DateTime.UtcNow;
                refreshToken.ReplacedByToken = replacedByToken;
                await _repository.UpdateAsync(refreshToken);
            }
        }

        public async Task RevokeAllUserTokensAsync(int userId)
        {
            var tokens = await _repository.GetUserTokensAsync(userId);
            foreach (var token in tokens)
            {
                token.IsRevoked = true;
                token.RevokedAt = DateTime.UtcNow;
            }
            await _repository.DeleteRangeAsync(tokens);
        }

        public async Task CleanupExpiredTokensAsync()
        {
                var allTokens = await _repository.GetUserTokensAsync(0); 
            var expiredTokens = allTokens.Where(t => t.IsExpired).ToList();
            if (expiredTokens.Any())
            {
                await _repository.DeleteRangeAsync(expiredTokens);
            }
        }
    }
}
