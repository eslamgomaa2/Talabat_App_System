using Domin.DTOS.DTO;
using Domin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using System;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserProfileRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            return user;
        }

        public async Task<ApplicationUser> EditUserAsync(string userId, UserDto userRequest)
        {
            var user = await GetUserByIdAsync(userId);

            user.FName = userRequest.FName;
            user.LName = userRequest.LName;
            user.Email = userRequest.Email;
            user.UserName = userRequest.UserName;
            user.PhoneNumber = userRequest.PhoneNumber;

            _dbContext.Update(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }
    }
}
