using Domin.DTOS.DTO;
using Domin.Models;

namespace Repository.Interfaces
{
    public interface IUserProfile
    {
        public Task<ApplicationUser> EditUser(string userid, UserDto userrequest);
    }
}
