using Domin.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;

namespace Repository.Implementation
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RefreshTokenRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RefreshToken> CreateAsync(int userId)
        {
            var refreshToken = new RefreshToken
            {
                UserId = userId,
                Token = GenerateRandomToken(),
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.RefreshTokens.AddAsync(refreshToken);
            await _dbContext.SaveChangesAsync();
            return refreshToken;
        }

        public async Task<RefreshToken?> GetValidTokenAsync(string token)
        {
            return await _dbContext.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == token && !t.IsRevoked && !t.IsExpired);
        }

        public async Task<RefreshToken?> GetByIdAsync(int id)
        {
            return await _dbContext.RefreshTokens.FindAsync(id);
        }

        public async Task<List<RefreshToken>> GetUserTokensAsync(int userId)
        {
            return await _dbContext.RefreshTokens
                .Where(t => t.UserId == userId && !t.IsRevoked && !t.IsExpired)
                .ToListAsync();
        }

        public async Task UpdateAsync(RefreshToken token)
        {
            _dbContext.RefreshTokens.Update(token);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(RefreshToken token)
        {
            _dbContext.RefreshTokens.Remove(token);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<RefreshToken> tokens)
        {
            _dbContext.RefreshTokens.RemoveRange(tokens);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        private string GenerateRandomToken()
        {
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                var randomNumber = new byte[64];
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}