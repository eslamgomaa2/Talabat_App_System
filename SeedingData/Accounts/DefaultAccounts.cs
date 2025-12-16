using Domin.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedingData.Accounts
{
    public static class DefaultAccounts
    {
        public static ApplicationUser AdminAccount()
        {
            return new ApplicationUser
            {
                Id = 1,
                FName = "Admin",
                LName = "Admin",
                UserName = "Admin",
                NormalizedUserName = "ADMIN@GMAIL.COM",
                Email = "Admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                EmailConfirmed = true,
                PhoneNumber = "012152001",
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null!, "Admin123!")

            };


        }
    }
}
