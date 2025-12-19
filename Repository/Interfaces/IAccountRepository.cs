using Domin.Enum;
using Domin.Models;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IAccountRepository
    {
        Task AddDriverAsync(Driver driver);
        Task AddRestaurantOwnerAsync(Resaurant_Owner owner);
    }
}
