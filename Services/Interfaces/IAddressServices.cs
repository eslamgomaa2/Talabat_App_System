using Domin.DTOS.DTO;
using Domin.Models;

namespace Repository.Interfaces
{
    public interface IAddressServices
    {
        public Task<Address> SetUserAddress(AddressDTO address);
        public Task<List<Address>> GetUserAddressAsync(string userid);
        public Task<Address> EditUserAddressAsync(string userid, AddressDTO request);
        public Task<Address> DeleteUserAddressAsync(string userid);

    }
}
