using Domin.DTOS.DTO;
using Domin.Enum;
using Domin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IDriverServices
    {
        Task<List<Driver>> GetAllDriversAsync();
        Task<Driver> GetDriverByID(int id);
        Task<List<Driver>> GetOnlyAvailableDrivers();
        Task<List<Driver>> FilterByVehicleType(Vehicles vehicletype);
        Task<Driver> AddNewDriver(DriverDto driver);
        Task<string> DeleteDriver (int id);
        Task<Driver> MarkDriverAsAvailable(int id);
        Task<Driver> MarkDriverAsUnAvailable(int id);
        Task<Driver> UpdateDriver(int id, DriverDto driver);
        Task<Driver> GetDriverActiveOrders (int id);
        Task<Driver> GetDriverCompletedOrders(int driverId);
        Task<Driver> FindDriverByphone (string phone );

    }
}
