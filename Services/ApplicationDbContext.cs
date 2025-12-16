using Domin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SeedingData.Accounts;
using SeedingData.Mapping;
using SeedingData.Roles;

namespace Repository
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<DeliveryDetail> DeliveryDetails { get; set; }
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<ApplicationUser> ApplictionUsers { get; set; }
        public DbSet<Resaurant_Owner> Resaurant_Owners { get; set; }
        public DbSet<Driver> Drivers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>(opt =>
            {
                opt.ToTable("Users");
                opt.HasKey(o => o.Id);
                opt.HasData(DefaultAccounts.AdminAccount());
            });
            modelBuilder.Entity<DeliveryDetail>(opt =>
            {
                opt.HasKey(o => o.DeliveryId);
                opt.ToTable("DeliveryDetail");
            });
            modelBuilder.Entity<IdentityRole>(opt =>
            {
                opt.ToTable("Roles");
                opt.HasData(DefaultRoles.RoleNames());
            
            });
            modelBuilder.Entity<IdentityUserRole<string>>(opt=>
            {
                opt.ToTable("UserRoles");
                opt.HasData(RoleMapping.AdminRoleMapping());

            }
            );
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");

        }
    }
}
