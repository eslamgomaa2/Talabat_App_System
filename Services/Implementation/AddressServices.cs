using Domin.DTOS.DTO;
using Domin.Models;
using Microsoft.AspNetCore.Http;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class AddressServices : IAddressServices
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IHttpContextAccessor _httpContext;

        public AddressServices(IAddressRepository addressRepository, IHttpContextAccessor httpContext)
        {
            _addressRepository = addressRepository;
            _httpContext = httpContext;
        }

        public async Task<List<Address>> GetUserAddressAsync(string userId)
        {
            var addresses = await _addressRepository.GetUserAddressesAsync(userId);
            if (addresses == null || !addresses.Any())
                throw new Exception("There are no addresses for this user.");

            return addresses;
        }

        public async Task<Address> SetUserAddress(AddressDTO request)
        {
            var userId = _httpContext.HttpContext.User.Claims
                                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                throw new Exception("User ID not found. Make sure the user is authenticated.");

            var address = MapToAddress(request);
            address.UserId = userId;

            await _addressRepository.AddAddressAsync(address);
            return address;
        }

        public async Task<Address> EditUserAddressAsync(string userId, AddressDTO request)
        {
            var address = await _addressRepository.GetAddressByUserIdAsync(userId)
                          ?? throw new Exception("No address found for this user.");

            address.AddressLine1 = request.AddressLine1;
            address.AddressLine2 = request.AddressLine2;
            address.City = request.City;
            address.State = request.State;
            address.PostalCode = request.PostalCode;
            address.Country = request.Country;

            await _addressRepository.UpdateAddressAsync(address);
            return address;
        }

        public async Task<Address> DeleteUserAddressAsync(string userId)
        {
            var address = await _addressRepository.GetAddressByUserIdAsync(userId)
                          ?? throw new Exception("No address found for this user.");

            await _addressRepository.DeleteAddressAsync(address);
            return address;
        }

        private Address MapToAddress(AddressDTO request)
        {
            return new Address
            {
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                City = request.City,
                State = request.State,
                PostalCode = request.PostalCode,
                Country = request.Country
            };
        }
    }
}
