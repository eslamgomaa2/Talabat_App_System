using Domin.DTOS.DTO;
using Domin.Enum;
using Domin.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class DriverServices : IDriverServices
    {
        private readonly ApplicationDbContext _dbcontext;

        public DriverServices(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<Driver> AddNewDriver(DriverDto driver)
        {
            var trimmedName = driver.Name?.Trim();
            var normalizedPhone = driver.PhoneNumber?.Trim();

            var existdriver = await _dbcontext.Drivers.SingleOrDefaultAsync(o => o.PhoneNumber==normalizedPhone && o.Name==trimmedName);
            if (existdriver != null) 
            {
                throw new InvalidOperationException("Driver is Already Exist");
            }
            var drivers = new Driver
            {
                Name = driver.Name,
                PhoneNumber= driver.PhoneNumber,
                CreatedAt=DateTime.Now,
                VehicleType= driver.VehicleType,
                VehicleRegistration = driver.VehicleRegistration,
                
            };
            await _dbcontext.Drivers.AddAsync(drivers);
            await _dbcontext.SaveChangesAsync();
            return drivers;


        }

       

        public async Task<string> DeleteDriver(int id)
        {
            var driver = await _dbcontext.Drivers.SingleOrDefaultAsync(o => o.DriverId == id) ?? throw new InvalidOperationException("Driver Doesn't Exist"); ;
           
            _dbcontext.Drivers.Remove(driver);  
            await _dbcontext.SaveChangesAsync();
            return "Driver Deleted Successfully";
        }

        public Task<List<Driver>> FilterByVehicleType(Vehicles vehicletype)
        {
            if (!Enum.IsDefined(typeof(Vehicles), vehicletype))
            {
                throw new ArgumentOutOfRangeException(nameof(vehicletype), "Invalid vehicle type.");
            }

            var drivers = _dbcontext.Drivers
                .Where(d => d.VehicleType == vehicletype)
                .ToListAsync();

            return drivers;
        }


        public async Task<Driver> FindDriverByphone(string phone)
        {
            var driver=await _dbcontext.Drivers.SingleOrDefaultAsync(o => o.PhoneNumber == phone.Trim())?? throw new InvalidOperationException("Driver Not Found");
            return driver;
        }

        public async Task<List<Driver>> GetAllDriversAsync()
        {
           var drivers=await _dbcontext.Drivers.ToListAsync();
            if (!drivers.Any()) 
            {
                throw new InvalidOperationException("No Drivers Found");
            }   
            return drivers;
        }

        public async Task<Driver> GetDriverCompletedOrders(int driverId)
        {
            var completedorderstodrover = await _dbcontext.Drivers
                .Include(d => d.DeliveryDetails)!.ThenInclude(x => x.Order)
                .SingleOrDefaultAsync(d => d.DriverId == driverId && d.DeliveryDetails!.Any(d => d.Status == DeliveryStatus.Delivered));
                

            if (completedorderstodrover is null)
            {
                throw new InvalidOperationException($"Driver with ID {driverId} not found.");
            }

            return completedorderstodrover;
        }

        public async Task<Driver> GetDriverActiveOrders(int id)
        {
            var driver = await _dbcontext.Drivers.Include(o => o.DeliveryDetails)!.ThenInclude(d => d.Order)
                .SingleOrDefaultAsync(o => o.DriverId == id && (o.DeliveryDetails!.Any(o => o.Status == DeliveryStatus.PickedUp || o.Status == DeliveryStatus.Assigned)));


            if (driver == null)
            {
                throw new InvalidOperationException("Driver Not Found or No Active Orders");
            }
            return driver;
        }

        public async Task<List<Driver>> GetOnlyAvailableDrivers()
        {
            var drivers=await _dbcontext.Drivers.Where(d => d.IsAvailable)
                .ToListAsync();
            if (!drivers.Any())
            {
                throw new InvalidOperationException("No Available Drivers Found");
            }
            return drivers;
        }

        public async Task<Driver> GetDriverByID(int id)
        {
           var driver = await _dbcontext.Drivers.SingleOrDefaultAsync(o => o.DriverId == id) ?? throw new InvalidOperationException("Driver Not Found");
            return driver;
        }

        public async Task<Driver> MarkDriverAsAvailable(int id)
        {
            var driver =await _dbcontext.Drivers.SingleOrDefaultAsync(o => o.DriverId == id) ?? throw new InvalidOperationException("Driver Not Found");
            driver.IsAvailable = true;
            await _dbcontext.SaveChangesAsync();
            return driver;
        }

        public async Task<Driver> MarkDriverAsUnAvailable(int id)
        {
            var driver =await _dbcontext.Drivers.SingleOrDefaultAsync(o => o.DriverId == id) ?? throw new InvalidOperationException("Driver Not Found");
            
            driver.IsAvailable = false;
            _dbcontext.Drivers.Update(driver);
            await _dbcontext.SaveChangesAsync();
            return driver;
        }

        public async Task<Driver> UpdateDriver(int id, DriverDto driver)
        {
           var getdriver= await _dbcontext.Drivers.SingleOrDefaultAsync(o=>o.DriverId==id)?? throw new InvalidOperationException("Driver Not Found");
           
            getdriver.Name = driver.Name?.Trim();
            getdriver.PhoneNumber = driver.PhoneNumber?.Trim();
            getdriver.VehicleType = driver.VehicleType;
            getdriver.VehicleRegistration = driver.VehicleRegistration;
            await _dbcontext.SaveChangesAsync();
            return getdriver;
        }
    }
}
