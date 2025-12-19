using Domin.DTOS.DTO;
using Domin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IAddressServices
    {
        public Task<Address> SetUserAddress (AddressDTO address);
        public Task<List<Address>> GetUserAddressAsync(string userid);
        public Task<Address> EditUserAddressAsync(string userid,AddressDTO request);
        public Task<Address> DeleteUserAddressAsync(string userid);
       
    }
}
