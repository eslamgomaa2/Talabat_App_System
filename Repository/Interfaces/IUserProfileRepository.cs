using Domin.DTOS.DTO;
using Domin.Models;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IUserProfileRepository
    {
        Task<ApplicationUser> EditUserAsync(string userId, UserDto userRequest);
        Task<ApplicationUser> GetUserByIdAsync(string userId);
    }
}
