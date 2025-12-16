using Domin.DTOS.DTO;
using Domin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class AddressServices : IAddressServices
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IHttpContextAccessor _httpcontext;

        public AddressServices(ApplicationDbContext dbcontext, IHttpContextAccessor httpcontext)
        {
            _dbcontext = dbcontext;
            _httpcontext = httpcontext;
        }

        public async Task<List<Address>> GetUserAddressAsync(string userid)
        {
            var Address = await _addressrepo.GetUserAddressAsync(userid);
            if (Address is null) {
                throw new Exception("there isno user with this id"); 
            }
            return Address;
        }


        public async Task<Address> SetUserAddress(AddressDTO request)
        {
            var userid = _httpcontext.HttpContext.User.Claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier);
            if (userid is null)
            {
                throw new Exception("User ID  not found. Make sure the user is authenticated.");
            }
            var address = MapingToAddress(request);
            address.UserId = userid.Value; 

            await _dbcontext.Addresses.AddAsync(address); 
            await _dbcontext.SaveChangesAsync();

            return address;
        }

       public  async Task<Address> DeleteUserAddressAsync(string userid)
        {
            var address = await _dbcontext.Addresses.SingleOrDefaultAsync(o => o.UserId == userid) ?? throw new Exception("There IS No User With This id"); ;
           
            _dbcontext.Addresses.Remove(address);   
            await _dbcontext.SaveChangesAsync();
            return address;
        }

        public async Task<Address> EditUserAddressAsync(string userid,AddressDTO request)
        {
            var address = await _dbcontext.Addresses.FirstOrDefaultAsync(o=>o.UserId==userid)?? throw new Exception("There IS No User With This id");
            
           address.AddressLine1= request.AddressLine1;
            address.AddressLine2= request.AddressLine2;
            address.City = request.City;
            address.State = request.State;
            address.PostalCode = request.PostalCode;

            await _dbcontext.SaveChangesAsync();
            return address;
        }

      

        private Address MapingToAddress(AddressDTO request) { 
            return new Address
            {
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                City = request.City,
                State = request.State,
                PostalCode = request.PostalCode,
                Country = request.Country,
                
            };




        }

        
    }
}
