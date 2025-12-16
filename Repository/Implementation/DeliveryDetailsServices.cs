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
    public class DeliveryDetailsServices : IDeliveryDetailsServices
    {
        private readonly ApplicationDbContext _dbcontext;

        public DeliveryDetailsServices(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

       

        public async Task<DeliveryDetail> Canceldelivery(int deleiveryid)
        {
           var delivery = await _dbcontext.DeliveryDetails.FindAsync(deleiveryid)??throw new Exception("Delivery Not Found");
            _dbcontext.DeliveryDetails.Remove(delivery);
            await _dbcontext.SaveChangesAsync();
            return delivery;
        }

        public async Task<DeliveryDetail> CreateNewDelivery(DeliveryDetailsDto request)
        {
            var delivery = new DeliveryDetail
            {
                OrderId = request.orderid,
                DriverId = request.driverid,
                PickupTime = DateTime.Now
            };
            _dbcontext.DeliveryDetails.Add(delivery);
            await _dbcontext.SaveChangesAsync(); 
            return delivery;
        }

        public async Task<List<DeliveryDetail>> DriversDeliveryHistory(int driverid)
        {
            var driverhistory=await _dbcontext.DeliveryDetails.Include(o=>o.Order).Where(d => d.DriverId == driverid).ToListAsync();
            if (driverhistory == null || !driverhistory.Any())
            {
                throw new InvalidOperationException("No delivery history found for this driver.");
            }
            return driverhistory;

        }

        public async Task<List<DeliveryDetail>> GetAllDeliveryDetailsAsync()
        {
            var deliveries = await _dbcontext.DeliveryDetails.Include(d => d.Driver).ToListAsync();
            if (deliveries == null || !deliveries.Any())
            {
                throw new InvalidOperationException("No delivery details found.");
            }
            return deliveries;
        }

        public async Task<List<DeliveryDetail>> GetAllDeliveryForADriver(int driverid)
        {
            var driverdelivery=await _dbcontext.DeliveryDetails
                .Include(d => d.Driver)
                .Include(d => d.Order)
                .Where(d => d.DriverId == driverid)
                .ToListAsync();
            if (driverdelivery == null || !driverdelivery.Any())
            {
                throw new InvalidOperationException("No delivery details found for this driver.");
            }
            return driverdelivery;
        }

        public async Task<DeliveryStatus> GetCurrentDeliveryStatus(int deliveryid)
        {
          var delivery=await _dbcontext.DeliveryDetails.SingleOrDefaultAsync(d => d.DeliveryId == deliveryid) ?? throw new InvalidOperationException("Delivery Not Found");
            return delivery.Status;
        }

        public async Task<DeliveryDetail> GetDeliveryByID(int id)
        {
           var delivery = await _dbcontext.DeliveryDetails
                .Include(d => d.Order)
                .Include(d => d.Driver)
                .SingleOrDefaultAsync(d => d.DeliveryId == id) ?? throw new InvalidOperationException("Delivery Not Found");
            return delivery;
        }

        public async Task<DeliveryDetail> GetDeliveryByOrderID(int orderid)
        {
            var delvery = await _dbcontext.DeliveryDetails
                .Include(d => d.Order)
                .Include(d => d.Driver)
                .SingleOrDefaultAsync(d => d.OrderId == orderid) ?? throw new InvalidOperationException("Delivery Not Found");
            return delvery;
        }

        public async Task<List<DeliveryDetail>> GetDeliveryDetailsByStatus(DeliveryStatus status)
        {
            var deliveries = await _dbcontext.DeliveryDetails
                .Include(d => d.Order)
                .Include(d => d.Driver)
                .Where(d => d.Status == status)
                .ToListAsync();
            if (deliveries == null || !deliveries.Any())
            {
                throw new InvalidOperationException("No delivery details found for the specified status.");
            }
            return deliveries;
        }

        public async Task<DeliveryDetail> MarkAsDeliveredt_setDeliveredTime(int deleiveryid)
        {
            var delvery = await _dbcontext.DeliveryDetails
                 .SingleOrDefaultAsync(d => d.DeliveryId == deleiveryid) ?? throw new InvalidOperationException("Delivery Not Found");
            delvery.DeliveredTime = DateTime.Now;
            delvery.Status = DeliveryStatus.Delivered;
            await _dbcontext.SaveChangesAsync();
            return delvery;
        }


        

       

        public async Task<DeliveryDetail> UpdateEntireDeliveryRecord(int deleiveryid, DeliveryDetailsDto request)
        {
           var delivery = await _dbcontext.DeliveryDetails.SingleOrDefaultAsync(o => o.DeliveryId == deleiveryid) ?? throw new InvalidOperationException("Delivery Not Found");
            if (delivery == null)
            {
                throw new InvalidOperationException("Delivery Not Found");
            }
            delivery.OrderId = request.orderid;
            delivery.DriverId = request.driverid;
            _dbcontext.DeliveryDetails.Update(delivery);
            await _dbcontext.SaveChangesAsync();
            return delivery;
        }

       public async Task<DeliveryStatus> UpdateDeliveryStatus(int id, DeliveryStatus status)
        {
            var delivery = await _dbcontext.DeliveryDetails.SingleOrDefaultAsync(o => o.DeliveryId == id) ?? throw new InvalidOperationException("Delivery Not Found");
            delivery.Status = status;
            await _dbcontext.SaveChangesAsync();
            return delivery.Status;
        }
    }
}
