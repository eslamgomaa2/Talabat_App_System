using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedingData.Mapping
{
    public static class RoleMapping
    {
        public static IdentityUserRole<string> AdminRoleMapping()
        {
            return new IdentityUserRole<string>
            {
                UserId = "1", 
                RoleId = "1"  
            };
        }
    }
}
