using Domin.Enum;
using Domin.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class DriverRepository : IDriverRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DriverRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Driver driver)
        {
            await _dbContext.Drivers.AddAsync(driver);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Driver driver)
        {
            _dbContext.Drivers.Remove(driver);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Driver>> FilterByVehicleTypeAsync(Vehicles vehicleType)
        {
            return await _dbContext.Drivers.Where(d => d.VehicleType == vehicleType).ToListAsync();
        }

        public async Task<List<Driver>> GetAllAsync()
        {
            return await _dbContext.Drivers.ToListAsync();
        }

        public async Task<List<Driver>> GetOnlyAvailableAsync()
        {
            return await _dbContext.Drivers.Where(d => d.IsAvailable).ToListAsync();
        }

        public async Task<Driver> GetByIdAsync(int id)
        {
            return await _dbContext.Drivers.SingleOrDefaultAsync(d => d.DriverId == id);
        }

        public async Task<Driver> FindByPhoneAsync(string phone)
        {
            return await _dbContext.Drivers.SingleOrDefaultAsync(d => d.PhoneNumber == phone.Trim());
        }

        public async Task<Driver> GetDriverWithCompletedOrders(int driverId)
        {
            return await _dbContext.Drivers
                .Include(d => d.DeliveryDetails).ThenInclude(x => x.Order)
                .SingleOrDefaultAsync(d => d.DriverId == driverId && d.DeliveryDetails.Any(o => o.Status == DeliveryStatus.Delivered));
        }

        public async Task<Driver> GetDriverWithActiveOrders(int driverId)
        {
            return await _dbContext.Drivers
                .Include(d => d.DeliveryDetails).ThenInclude(x => x.Order)
                .SingleOrDefaultAsync(d => d.DriverId == driverId && d.DeliveryDetails.Any(o => o.Status == DeliveryStatus.Assigned || o.Status == DeliveryStatus.PickedUp));
        }

        public async Task UpdateAsync(Driver driver)
        {
            _dbContext.Drivers.Update(driver);
            await _dbContext.SaveChangesAsync();
        }
    }
}
