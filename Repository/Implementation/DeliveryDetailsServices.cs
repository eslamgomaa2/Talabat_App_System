using Domin.DTOS.DTO;
using Domin.Enum;
using Domin.Models;
using Microsoft.Extensions.Caching.Memory;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class DeliveryDetailsServices : IDeliveryDetailsServices
    {
        private readonly IDeliveryDetailsRepository _repository;
        private readonly IMemoryCache _cache;

        public DeliveryDetailsServices(IDeliveryDetailsRepository repository, IMemoryCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<DeliveryDetail> CreateNewDelivery(DeliveryDetailsDto request)
        {
            var delivery = new DeliveryDetail
            {
                OrderId = request.orderid,
                DriverId = request.driverid,
                PickupTime = DateTime.Now,
                Status = DeliveryStatus.Assigned
            };

            await _repository.AddAsync(delivery);
            return delivery;
        }

        public async Task<DeliveryDetail> CancelDelivery(int deliveryId)
        {
            var delivery = await _repository.GetByIdAsync(deliveryId)
                           ?? throw new InvalidOperationException("Delivery Not Found");

            await _repository.DeleteAsync(delivery);
            return delivery;
        }

        public async Task<List<DeliveryDetail>> DriversDeliveryHistory(int driverId)
        {
            var history = await _repository.GetByDriverIdAsync(driverId);
            if (history == null || history.Count == 0)
                throw new InvalidOperationException("No delivery history found for this driver.");

            return history;
        }

        public async Task<List<DeliveryDetail>> GetAllDeliveryDetailsAsync()
        {
            const string cacheKey = "delivery_details_all";

            if (_cache.TryGetValue(cacheKey, out List<DeliveryDetail> cachedDeliveries))
                return cachedDeliveries;

            var deliveries = await _repository.GetAllAsync();

            if (deliveries == null || deliveries.Count == 0)
                throw new InvalidOperationException("No delivery details found.");

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(3)
            };

            _cache.Set(cacheKey, deliveries, cacheOptions);

            return deliveries;
        }


        public async Task<List<DeliveryDetail>> GetAllDeliveryForADriver(int driverId)
        {
            var deliveries = await _repository.GetByDriverIdAsync(driverId);
            if (deliveries == null || deliveries.Count == 0)
                throw new InvalidOperationException("No delivery details found for this driver.");

            return deliveries;
        }

        public async Task<DeliveryStatus> GetCurrentDeliveryStatus(int deliveryId)
        {
            var delivery = await _repository.GetByIdAsync(deliveryId)
                           ?? throw new InvalidOperationException("Delivery Not Found");

            return delivery.Status;
        }

        public async Task<DeliveryDetail> GetDeliveryByID(int id)
        {
            var delivery = await _repository.GetByIdAsync(id)
                           ?? throw new InvalidOperationException("Delivery Not Found");

            return delivery;
        }

        public async Task<DeliveryDetail> GetDeliveryByOrderID(int orderId)
        {
            var delivery = await _repository.GetByOrderIdAsync(orderId)
                           ?? throw new InvalidOperationException("Delivery Not Found");

            return delivery;
        }

        public async Task<List<DeliveryDetail>> GetDeliveryDetailsByStatus(DeliveryStatus status)
        {
            var deliveries = await _repository.GetByStatusAsync(status);
            if (deliveries == null || deliveries.Count == 0)
                throw new InvalidOperationException("No delivery details found for the specified status.");

            return deliveries;
        }


        public async Task<DeliveryDetail> UpdateEntireDeliveryRecord(int deliveryId, DeliveryDetailsDto request)
        {
            var delivery = await _repository.GetByIdAsync(deliveryId)
                           ?? throw new InvalidOperationException("Delivery Not Found");

            delivery.OrderId = request.orderid;
            delivery.DriverId = request.driverid;

            await _repository.UpdateAsync(delivery);
            return delivery;
        }

        public async Task<DeliveryStatus> UpdateDeliveryStatus(int deliveryId, DeliveryStatus status)
        {
            var delivery = await _repository.GetByIdAsync(deliveryId)
                           ?? throw new InvalidOperationException("Delivery Not Found");

            delivery.Status = status;
            await _repository.UpdateAsync(delivery);
            return delivery.Status;
        }

        public async Task<DeliveryDetail> MarkAsDeliveredt_setDeliveredTime(int deleiveryid)
        {
            var delivery = await _repository.GetByIdAsync(deleiveryid)
                          ?? throw new InvalidOperationException("Delivery Not Found");

            delivery.DeliveredTime = DateTime.Now;
            delivery.Status = DeliveryStatus.Delivered;

            await _repository.UpdateAsync(delivery);
            return delivery;
        }

        public async Task<DeliveryDetail> Canceldelivery(int deleiveryid)
        {
            var delivery = await _repository.GetByIdAsync(deleiveryid)
                           ?? throw new InvalidOperationException("Delivery Not Found");

            await _repository.DeleteAsync(delivery);
            return delivery;
        
        }
    }
}
