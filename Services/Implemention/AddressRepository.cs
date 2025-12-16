using Domin.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AddressRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Address>> GetUserAddressesAsync(string userId)
        {
            return await _dbContext.Addresses
                                   .Where(a => a.UserId == userId)
                                   .ToListAsync();
        }

        public async Task<Address> GetAddressByUserIdAsync(string userId)
        {
            return await _dbContext.Addresses.FirstOrDefaultAsync(a => a.UserId == userId);
        }

        public async Task AddAddressAsync(Address address)
        {
            await _dbContext.Addresses.AddAsync(address);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAddressAsync(Address address)
        {
            _dbContext.Addresses.Update(address);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAddressAsync(Address address)
        {
            _dbContext.Addresses.Remove(address);
            await _dbContext.SaveChangesAsync();
        }
    }
}
