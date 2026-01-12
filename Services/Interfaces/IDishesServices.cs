using Domin.DTOS.DTO;
using Domin.Models;

namespace Repository.Interfaces
{
    public interface IDishesServices
    {
        public Task<List<Dish>> GetAllDishes();
        public Task<Dish> AddDishForSpecificRestaurant(DishDto dish);
        public Task<Dish> UpdateDish(int id, DishDto dish);
        public Task<Dish> DeletDish(int id);
        public Task<Dish> GetDishById(int id);
        public Task<List<Dish>> GetDishesByName(string name);
        public Task<List<Dish>> GetOnlyAvailableDishesForRestaurant(int id);
    }
}
