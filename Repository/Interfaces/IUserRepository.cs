using Domin.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetByEmailAsync(string email);
        Task<ApplicationUser?> GetByUsernameAsync(string username);
        Task<ApplicationUser?> GetByIdAsync(string id);
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        Task AddToRoleAsync(ApplicationUser user, string role);
        Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword);
    }
}
