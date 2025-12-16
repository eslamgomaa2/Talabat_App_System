
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Models
{
    /* Identityuser<int> made id datatybe is int instude of string*/
    /* identityuser: Identityuser<string> this which made datatype string*/
    public class ApplicationUser : IdentityUser<int>    

    {

        public string? FName { get; set; }
        public string? LName { get; set; }

        // Navigation properties
        public ICollection<Address>? Addresses { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}
