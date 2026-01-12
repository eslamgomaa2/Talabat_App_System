using Domin.DTOS.DTO;
using Domin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Repository.Interfaces;
using System.Security.Claims;

namespace Repository.Implementation
{
    public class DishesServices : IDishesServices
    {
        private readonly IDishRepository _dishRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _cache;

        public DishesServices(IDishRepository dishRepository, IHttpContextAccessor httpContextAccessor, IMemoryCache cache)
        {
            _dishRepository = dishRepository;
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
        }



        public async Task<Dish> AddDishForSpecificRestaurant(DishDto request)
        {
            var ownerId = _httpContextAccessor.HttpContext?.User?.Claims
                            ?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
                            ?? throw new Exception("Unauthorized");


            var exists = await _dishRepository.RestaurantExistsAsync(request.RestaurantId);
            if (!exists)
                throw new KeyNotFoundException("No restaurant found with this Id");

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

            await _dishRepository.AddAsync(dish);
            return dish;
        }

        public Task<Dish> DeletDish(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Dish> DeleteDish(int id)
        {
            var dish = await _dishRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Dish not found");
            await _dishRepository.DeleteAsync(dish);
            return dish;
        }

        public async Task<List<Dish>> GetAllDishes()
        {
            const string cacheKey = "all_dishes";
            if (_cache.TryGetValue(cacheKey, out List<Dish> alldishes))
                return alldishes;

            var dishes = await _dishRepository.GetAllAsync();
            if (dishes.Count == 0)
                throw new KeyNotFoundException("No dishes found");
            _cache.Set(cacheKey, alldishes, TimeSpan.FromMinutes(5));
            return dishes;
        }

        public async Task<Dish> GetDishById(int id)
        {
            var dish = await _dishRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Dish not found");
            return dish;
        }

        public async Task<List<Dish>> GetDishesByName(string name)
        {
            var dishes = await _dishRepository.GetByNameAsync(name);
            if (dishes.Count == 0) throw new KeyNotFoundException("Dish not found");
            return dishes;
        }

        public async Task<List<Dish>> GetOnlyAvailableDishesForRestaurant(int restaurantId)
        {
            var dishes = await _dishRepository.GetAvailableForRestaurantAsync(restaurantId);
            if (dishes.Count == 0)
                throw new KeyNotFoundException("No available dishes found for this restaurant.");
            return dishes;
        }

        public async Task<Dish> UpdateDish(int id, DishDto request)
        {
            var dish = await _dishRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Dish not found");

            dish.Name = request.Name;
            dish.Price = request.Price;
            dish.Category = request.Category;
            dish.CreatedAt = request.CreatedAt;
            dish.UpdatedAt = request.UpdatedAt;
            dish.Description = request.Description;
            dish.IsAvailable = request.IsAvailable;

            await _dishRepository.UpdateAsync(dish);
            return dish;
        }


    }
}
