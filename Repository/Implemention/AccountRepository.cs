using Domin.Models;
using Repository.Interfaces;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AccountRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddDriverAsync(Driver driver)
        {
            await _dbContext.Drivers.AddAsync(driver);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddRestaurantOwnerAsync(Resaurant_Owner owner)
        {
            await _dbContext.Resaurant_Owners.AddAsync(owner);
            await _dbContext.SaveChangesAsync();
        }
    }
}
