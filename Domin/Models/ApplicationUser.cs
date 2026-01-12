
using Microsoft.AspNetCore.Identity;

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
