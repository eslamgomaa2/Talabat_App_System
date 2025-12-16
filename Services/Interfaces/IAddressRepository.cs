using Domin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IAddressRepository
    {
        Task<List<Address>> GetUserAddressesAsync(string userId);
        Task<Address> GetAddressByUserIdAsync(string userId);
        Task AddAddressAsync(Address address);
        Task UpdateAddressAsync(Address address);
        Task DeleteAddressAsync(Address address);
    }
}
