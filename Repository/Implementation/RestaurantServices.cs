using Domin.DTOS.DTO;
using Domin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class RestaurantServices : IRestaurantServices
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IHttpContextAccessor _httpcontext;

        public RestaurantServices(ApplicationDbContext context, IHttpContextAccessor httpcontext)
        {
            _dbcontext = context;
          
            _httpcontext = httpcontext;
        }

        public async Task<Restaurant> AddRestaurantAsync(RestaurantDTO request)
        {
            var ownerIdClaim = _httpcontext.HttpContext?.User?.Claims?.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;

            if (ownerIdClaim is null)
            {
                throw new Exception("There is a Problem While I Get UserId ");
            }
            var restaurant = MapToRestaurant(request);
            var owner = _dbcontext.Resaurant_Owners.SingleOrDefaultAsync(o => o.UserId == ownerIdClaim);


            if (owner is null)
            {
                throw new Exception("There IS NO OWner With This id   ");
            }
            restaurant.OwnerId = owner.Id; 
            _dbcontext.Restaurants.Add(restaurant);
            await _dbcontext.SaveChangesAsync();
            return restaurant;
        }

        public async Task<Restaurant> DeletRestaurantAsync(int id)
        {
            var restaurant =await _dbcontext.Restaurants.FindAsync(id);
            if (restaurant == null)
            {
                throw new KeyNotFoundException("Restaurant not found");
            }
             _dbcontext.Restaurants.Remove(restaurant);
            await _dbcontext.SaveChangesAsync();
            return restaurant;
        }

        public async Task<Restaurant> EditRestaurantAsync(int id, RestaurantDTO request)
        {
            var restaurant= await _dbcontext.Restaurants.FindAsync(id)?? throw new KeyNotFoundException("Restaurant not found"); ;
            restaurant.Name = request.Name;
            restaurant.Description = request.Description;
            restaurant.CuisineType = request.CuisineType;
            restaurant.ContactPhone = request.ContactPhone;
            restaurant.OpeningTime = request.OpeningTime;
            restaurant.ClosingTime = request.ClosingTime;
            restaurant.IsActive = request.IsActive;
            restaurant.UpdatedAt = DateTime.Now;

            await _dbcontext.SaveChangesAsync();
            return restaurant;
        }

        public async Task<List<Address>> GetAddressesforRestaurant(int id)
        {
           var addresses=await _dbcontext.Addresses.Where(o=>o.RestaurantId==id).ToListAsync();
            if (addresses == null || !addresses.Any())
            {
                throw new KeyNotFoundException("No addresses found for this restaurant");
            }
            return addresses;
        }

        public Task<List<Restaurant>> GetAllRestaurantsforOwner(int id)
        {
            var restaurants = _dbcontext.Restaurants.Where(o => o.OwnerId == id); 
            if (restaurants == null || !restaurants.Any())
            {
                throw new KeyNotFoundException("No restaurants found for this owner");
            }
            return restaurants.ToListAsync();

        }

        public async Task<List<Dish>> GetDishesforRestaurant(int id)
        {
            var dishesofrestaurant=await _dbcontext.Restaurants.Include(r => r.Dishes).SingleOrDefaultAsync(r => r.RestaurantId == id);
            if (dishesofrestaurant is null)
            {
                throw new KeyNotFoundException("No dishes found for this restaurant");
            }
            return dishesofrestaurant.Dishes!.ToList();
        }

        public async Task<List<Restaurant>> GetRestaurantsByCuisineType(string CuisineType)
        {
            var restaurants= await _dbcontext.Restaurants.Where(r => r.CuisineType != null && r.CuisineType==CuisineType).ToListAsync();
            if (restaurants == null || !restaurants.Any())
            {
                throw new KeyNotFoundException("No restaurants found for this cuisine type");
            }
            return restaurants;
        }

        public async Task<List<Restaurant>> GetRestaurantByName(string name)
        {
            var restaurants=await _dbcontext.Restaurants.Where(r => r.Name != null && r.Name==name).ToListAsync();
            if (restaurants == null || !restaurants.Any())
            {
                throw new KeyNotFoundException("No restaurants found with this name");
            }
            return restaurants;
        }

        public async Task<List<Restaurant>> GetAllRestaurants()
        {
            var restaurant= await _dbcontext.Restaurants.ToListAsync();    
            if(restaurant is null || !restaurant.Any()) 
            {
                throw new Exception("There Is No Restaurant");
            }
            return restaurant;
        }



        private Restaurant MapToRestaurant(RestaurantDTO request)
        {
            return new Restaurant
            {
                
                Name = request.Name,
                Description = request.Description,
                CuisineType = request.CuisineType,
                ContactPhone = request.ContactPhone,
                OpeningTime = request.OpeningTime,
                ClosingTime = request.ClosingTime,
                IsActive = request.IsActive,
                CreatedAt = DateTime.Now,
              
                
            };
        }

        
    }
}
