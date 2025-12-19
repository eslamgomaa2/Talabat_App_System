using Domin.Models;
using Microsoft.AspNetCore.Identity;
using Repository.Interfaces;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public Task<ApplicationUser?> GetByEmailAsync(string email) => _userManager.FindByEmailAsync(email);

        public Task<ApplicationUser?> GetByUsernameAsync(string username) => _userManager.FindByNameAsync(username);

        public Task<ApplicationUser?> GetByIdAsync(string id) => _userManager.FindByIdAsync(id);

        public Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
            => _userManager.CreateAsync(user, password);

        public Task AddToRoleAsync(ApplicationUser user, string role)
            => _userManager.AddToRoleAsync(user, role);

        public Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
            => _userManager.ResetPasswordAsync(user, token, newPassword);
    }
}
