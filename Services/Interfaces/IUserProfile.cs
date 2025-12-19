using Domin.DTOS.DTO;
using Domin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
     public interface IUserProfile
    {
        public  Task<ApplicationUser> EditUser(string userid, UserDto userrequest);
    }
}
