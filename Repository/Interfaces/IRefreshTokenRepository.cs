using Domin.Models;

namespace Repository.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> CreateAsync(int userId);
        Task<RefreshToken?> GetValidTokenAsync(string token);
        Task<RefreshToken?> GetByIdAsync(int id);
        Task<List<RefreshToken>> GetUserTokensAsync(int userId);
        Task UpdateAsync(RefreshToken token);
        Task DeleteAsync(RefreshToken token);
        Task DeleteRangeAsync(IEnumerable<RefreshToken> tokens);
        Task SaveChangesAsync();
    }
}