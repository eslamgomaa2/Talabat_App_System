using Domin.DTOS.DTO;
using Domin.Models;
using Repository.Interfaces;

namespace Repository.Implementation
{
    public class UserProfileService : IUserProfile
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public UserProfileService(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public Task<ApplicationUser> EditUser(string userId, UserDto userRequest)
        {
            return _userProfileRepository.EditUserAsync(userId, userRequest);
        }
    }
}
