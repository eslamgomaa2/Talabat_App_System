using Domin.DTOS.DTO;
using Domin.Enum;
using Domin.Models;

namespace Repository.Interfaces
{
    public interface IDeliveryDetailsServices
    {
        public Task<List<DeliveryDetail>> GetAllDeliveryDetailsAsync();
        public Task<DeliveryDetail> GetDeliveryByID(int id);
        public Task<DeliveryDetail> GetDeliveryByOrderID(int orderid);
        public Task<List<DeliveryDetail>> GetAllDeliveryForADriver(int driverid);
        public Task<DeliveryDetail> CreateNewDelivery(DeliveryDetailsDto request);
        public Task<DeliveryDetail> UpdateEntireDeliveryRecord(int deleiveryid, DeliveryDetailsDto request);
        public Task<DeliveryStatus> UpdateDeliveryStatus(int id, DeliveryStatus status);
        public Task<DeliveryDetail> MarkAsDeliveredt_setDeliveredTime(int deleiveryid);
        public Task<DeliveryDetail> Canceldelivery(int deleiveryid);
        public Task<DeliveryStatus> GetCurrentDeliveryStatus(int deliveryid);
        public Task<List<DeliveryDetail>> GetDeliveryDetailsByStatus(DeliveryStatus status);
        public Task<List<DeliveryDetail>> DriversDeliveryHistory(int driverid);


    }
}
