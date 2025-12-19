using Domin.Enum;
using Domin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IDeliveryDetailsRepository
    {
        Task<DeliveryDetail> GetByIdAsync(int deliveryId);
        Task<DeliveryDetail> GetByOrderIdAsync(int orderId);
        Task<List<DeliveryDetail>> GetAllAsync();
        Task<List<DeliveryDetail>> GetByDriverIdAsync(int driverId);
        Task<List<DeliveryDetail>> GetByStatusAsync(DeliveryStatus status);
        Task AddAsync(DeliveryDetail delivery);
        Task UpdateAsync(DeliveryDetail delivery);
        Task DeleteAsync(DeliveryDetail delivery);
    }
}
