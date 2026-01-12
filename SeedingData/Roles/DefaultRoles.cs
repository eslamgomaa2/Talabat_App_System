using Microsoft.AspNetCore.Identity;

namespace SeedingData.Roles
{
    public static class DefaultRoles
    {
        public static List<IdentityRole> RoleNames()
        {
            return new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = "1",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = "Customer",
                    NormalizedName = "CUSTOMER"
                },
                new IdentityRole
                {
                    Id = "3",
                    Name = "RestaurantOwner",
                    NormalizedName = "RESTAURANTOWNER"
                },
                new IdentityRole
                {
                    Id = "4",
                    Name = "Driver",
                    NormalizedName = "DRIVER"
                }
            };
        }
    }
}