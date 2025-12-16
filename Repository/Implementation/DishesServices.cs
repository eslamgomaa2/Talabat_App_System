using Domin.DTOS.DTO;
using Domin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using System.Security.Claims;

namespace Repository.Implementation
{
    public class DishesServices : IDishesServices
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DishesServices(ApplicationDbContext dbcontext, IHttpContextAccessor httpContextAccessor)
        {
            _dbcontext = dbcontext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Dish> AddaDishForASpecificRrestaurant(DishDto request)
        {
            var ownerId = _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value
                ?? throw new Exception("Unauthorized");

            var restaurantOwner = await _dbcontext.Resaurant_Owners.SingleOrDefaultAsync(o => o.UserId == ownerId)
                ?? throw new Exception("Owner not found");

            var restaurant = await _dbcontext.Restaurants.SingleOrDefaultAsync(r => r.OwnerId == restaurantOwner.Id && r.RestaurantId == request.RestaurantId)
                ?? throw new Exception("You do not own this restaurant");

            if (request.ImageFile == null)
                throw new Exception("Image cannot be null");

            
            using var memoryStream = new MemoryStream();
            request.ImageFile.CopyTo(memoryStream);
            byte[] imageBytes = memoryStream.ToArray();

            var dish = new Dish
            {
                Name = request.Name,
                Price = request.Price,
                Category = request.Category,
                CreatedAt = request.CreatedAt,
                UpdatedAt = request.UpdatedAt,
                Description = request.Description,
                Image = imageBytes,
                IsAvailable = request.IsAvailable,
                RestaurantId = request.RestaurantId
            };

            await _dbcontext.Dishes.AddAsync(dish);
            await _dbcontext.SaveChangesAsync();
            return dish;
        }

        public async Task<Dish> DeletDish(int id)
        {
            var dish = await _dbcontext.Dishes.FindAsync(id) ?? throw new KeyNotFoundException("Dish not found");
            _dbcontext.Dishes.Remove(dish);
            await _dbcontext.SaveChangesAsync();
            return dish;
        }

        public async Task<List<Dish>> GetAllDishes()
        {
            var dishes = await _dbcontext.Dishes.ToListAsync();
            if (dishes.Count == 0)
            {
                throw new KeyNotFoundException("No dishes found");
            }
            return dishes;
        }

        public async Task<Dish> GetDishById(int id)
        {
            var dish = await _dbcontext.Dishes.Include(d => d.Restaurant).SingleOrDefaultAsync(d => d.DishId == id)
                ?? throw new KeyNotFoundException("Dish not found");
            return dish;
        }

        public async Task<List<Dish>> GetDishesByName(string name)
        {
            var dishes = await _dbcontext.Dishes.Include(d => d.Restaurant).Where(d => d.Name == name).ToListAsync();
            if (dishes.Count == 0)
            {
                throw new KeyNotFoundException("Dish not found");
            }
            return dishes;
        }

        public async Task<List<Dish>> GetOnlyAvailableDishesForRestaurant(int restaurantId)
        {
            var isrestaurantexist = await _dbcontext.Restaurants.FindAsync(restaurantId)?? throw new KeyNotFoundException("No Restaurant With This Id"); ;
            var dishes = await _dbcontext.Dishes.Include(d => d.Restaurant).Where(d => d.RestaurantId == restaurantId && d.IsAvailable).ToListAsync();
            if (dishes.Count == 0)
            {
                throw new KeyNotFoundException("No available dishes Found For This Restaurant.");
            }
            return dishes;
        }

        public async Task<Dish> UpdateDishes(int id, DishDto request)
        {
            var dish = await _dbcontext.Dishes.FindAsync(id) ?? throw new KeyNotFoundException("Dish not found");

            dish.Name = request.Name;
            dish.Price = request.Price;
            dish.Category = request.Category;
            dish.CreatedAt = request.CreatedAt;
            dish.UpdatedAt = request.UpdatedAt;
            dish.Description = request.Description;
            dish.IsAvailable = request.IsAvailable;

            _dbcontext.Dishes.Update(dish);
            await _dbcontext.SaveChangesAsync();
            return dish;
        }
    }
}
