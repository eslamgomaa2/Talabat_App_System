using Domin.Enum;
using Domin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IDriverRepository
    {
        Task<Driver> GetByIdAsync(int id);
        Task<List<Driver>> GetAllAsync();
        Task<List<Driver>> GetOnlyAvailableAsync();
        Task<List<Driver>> FilterByVehicleTypeAsync(Vehicles vehicleType);
        Task<Driver> FindByPhoneAsync(string phone);
        Task AddAsync(Driver driver);
        Task UpdateAsync(Driver driver);
        Task DeleteAsync(Driver driver);
        Task<Driver> GetDriverWithCompletedOrders(int driverId);
        Task<Driver> GetDriverWithActiveOrders(int driverId);
    }
}
