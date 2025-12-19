using Domin.DTOS.DTO;
using Domin.Enum;
using Domin.Helper;
using Domin.Models;
using Microsoft.Extensions.Caching.Memory;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class DriverServices : IDriverServices
    {
        private readonly IDriverRepository _driverRepository;
        private readonly IMemoryCache _cashe;

        public DriverServices(IDriverRepository driverRepository, IMemoryCache cashe)
        {
            _driverRepository = driverRepository;
            _cashe = cashe;
        }

        public async Task<Driver> AddNewDriver(DriverDto driver)
        {
            var trimmedName = driver.Name?.Trim();
            var normalizedPhone = driver.PhoneNumber?.Trim();

            var existDriver = await _driverRepository.FindByPhoneAsync(normalizedPhone);
            if (existDriver != null && existDriver.Name == trimmedName)
                throw new InvalidOperationException("Driver already exists");

            var newDriver = new Driver
            {
                Name = driver.Name,
                PhoneNumber = driver.PhoneNumber,
                CreatedAt = DateTime.Now,
                VehicleType = driver.VehicleType,
                VehicleRegistration = driver.VehicleRegistration
            };

            await _driverRepository.AddAsync(newDriver);
            return newDriver;
        }

        public async Task<string> DeleteDriver(int id)
        {
            var driver = await _driverRepository.GetByIdAsync(id) ?? throw new InvalidOperationException("Driver doesn't exist");
            await _driverRepository.DeleteAsync(driver);
            return "Driver deleted successfully";
        }

        public async Task<List<Driver>> FilterByVehicleType(Vehicles vehicleType)
        {
            if (!Enum.IsDefined(typeof(Vehicles), vehicleType))
                throw new ArgumentOutOfRangeException(nameof(vehicleType), "Invalid vehicle type.");

            return await _driverRepository.FilterByVehicleTypeAsync(vehicleType);
        }
        public async Task<Driver> FindDriverByphone(string phone)
        {
            var driver = await _driverRepository.FindByPhoneAsync(phone)
                         ?? throw new InvalidOperationException("Driver not found");
            return driver;
        }

        public async Task<List<Driver>> GetAllDriversAsync()
        {
            string cacheKey = "all_drivers";
            if(_cashe.TryGetValue(cacheKey,out List<Driver> alldrivers))
                return alldrivers;
            var drivers = await _driverRepository.GetAllAsync();
            if (drivers.Count == 0) throw new InvalidOperationException("No drivers found");
            _cashe.Set(cacheKey, drivers, TimeSpan.FromMinutes(10));
            
            return drivers;
        }

        public async Task<Driver> GetDriverCompletedOrders(int driverId)
        {
            var driver = await _driverRepository.GetDriverWithCompletedOrders(driverId)
                         ?? throw new InvalidOperationException($"Driver with ID {driverId} not found.");
            return driver;
        }

        public async Task<Driver> GetDriverActiveOrders(int driverId)
        {
            var driver = await _driverRepository.GetDriverWithActiveOrders(driverId)
                         ?? throw new InvalidOperationException("Driver not found or no active orders");
            return driver;
        }

        public async Task<List<Driver>> GetOnlyAvailableDrivers()
        {
            var drivers = await _driverRepository.GetOnlyAvailableAsync();
            if (drivers.Count == 0) throw new InvalidOperationException("No available drivers found");
            return drivers;
        }

        public async Task<Driver> GetDriverByID(int id)
        {
            var driver = await _driverRepository.GetByIdAsync(id)
                         ?? throw new InvalidOperationException("Driver not found");
            return driver;
        }

        public async Task<Driver> MarkDriverAsAvailable(int id)
        {
            var driver = await _driverRepository.GetByIdAsync(id)
                         ?? throw new InvalidOperationException("Driver not found");
            driver.IsAvailable = true;
            await _driverRepository.UpdateAsync(driver);
            return driver;
        }
        public async Task<Driver> MarkDriverAsUnAvailable(int id)
        {
            var driver = await _driverRepository.GetByIdAsync(id)
                         ?? throw new InvalidOperationException("Driver not found");
            driver.IsAvailable = false;
            await _driverRepository.UpdateAsync(driver);
            return driver;
        }

        public async Task<Driver> UpdateDriver(int id, DriverDto driver)
        {
            var existingDriver = await _driverRepository.GetByIdAsync(id)
                                 ?? throw new InvalidOperationException("Driver not found");

            existingDriver.Name = driver.Name?.Trim();
            existingDriver.PhoneNumber = driver.PhoneNumber?.Trim();
            existingDriver.VehicleType = driver.VehicleType;
            existingDriver.VehicleRegistration = driver.VehicleRegistration;

            await _driverRepository.UpdateAsync(existingDriver);
            return existingDriver;
        }

        
       

       
        
    }
}
