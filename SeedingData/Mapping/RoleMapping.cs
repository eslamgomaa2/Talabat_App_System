using Microsoft.AspNetCore.Identity;

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
