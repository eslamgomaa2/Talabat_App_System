using Domin.DTOS.DTO;
using Domin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class UserProfile : IUserProfile
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserProfile(UserManager<ApplicationUser> userManager, ApplicationDbContext dbcontext)
        {
            this._userManager = userManager;
            _dbcontext = dbcontext;
        }

        public async Task<ApplicationUser> EditUser(string userid,UserDto userrequest)
        {
            var user = await _userManager.FindByIdAsync(userid);
            if (user is null) {
                throw new Exception("there is no user ");
            }
            user.FName= userrequest.FName;
            user.LName = userrequest.LName;
            user.Email= userrequest.Email;
            user.UserName= userrequest.UserName;
            user.PhoneNumber= userrequest.PhoneNumber;
             _dbcontext.Update(user);
            await _dbcontext.SaveChangesAsync();
            return user;
           
            
        }
       

       
    }
}
