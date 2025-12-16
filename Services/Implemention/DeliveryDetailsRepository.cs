using Domin.Enum;
using Domin.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class DeliveryDetailsRepository : IDeliveryDetailsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DeliveryDetailsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DeliveryDetail> GetByIdAsync(int deliveryId)
        {
            return await _dbContext.DeliveryDetails
                .Include(d => d.Driver)
                .Include(d => d.Order)
                .SingleOrDefaultAsync(d => d.DeliveryId == deliveryId);
        }

        public async Task<DeliveryDetail> GetByOrderIdAsync(int orderId)
        {
            return await _dbContext.DeliveryDetails
                .Include(d => d.Driver)
                .Include(d => d.Order)
                .SingleOrDefaultAsync(d => d.OrderId == orderId);
        }

        public async Task<List<DeliveryDetail>> GetAllAsync()
        {
            return await _dbContext.DeliveryDetails
                .Include(d => d.Driver)
                .Include(d => d.Order)
                .ToListAsync();
        }

        public async Task<List<DeliveryDetail>> GetByDriverIdAsync(int driverId)
        {
            return await _dbContext.DeliveryDetails
                .Include(d => d.Driver)
                .Include(d => d.Order)
                .Where(d => d.DriverId == driverId)
                .ToListAsync();
        }

        public async Task<List<DeliveryDetail>> GetByStatusAsync(DeliveryStatus status)
        {
            return await _dbContext.DeliveryDetails
                .Include(d => d.Driver)
                .Include(d => d.Order)
                .Where(d => d.Status == status)
                .ToListAsync();
        }

        public async Task AddAsync(DeliveryDetail delivery)
        {
            await _dbContext.DeliveryDetails.AddAsync(delivery);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(DeliveryDetail delivery)
        {
            _dbContext.DeliveryDetails.Update(delivery);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(DeliveryDetail delivery)
        {
            _dbContext.DeliveryDetails.Remove(delivery);
            await _dbContext.SaveChangesAsync();
        }
    }
}
