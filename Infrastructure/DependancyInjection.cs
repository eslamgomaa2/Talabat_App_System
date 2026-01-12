using Microsoft.Extensions.DependencyInjection;
using Repository.Implementation;
using Repository.Interfaces;

namespace Infrastructure
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection Services)
        {
            Services.AddScoped<IDishesServices, DishesServices>();
            Services.AddScoped<IDriverServices, DriverServices>();
            Services.AddScoped<IOrderDishServices, OrderDishServices>();
            Services.AddScoped<IOrderServices, OrderServices>();
            Services.AddScoped<IRestaurantServices, RestaurantServices>();
            Services.AddScoped<IUserProfile, UserProfileService>();
            Services.AddScoped<IAccountServices, AccountServices>();
            Services.AddScoped<IEmailService, EmailService>();
            Services.AddScoped<IDeliveryDetailsServices, DeliveryDetailsServices>();
            Services.AddScoped<IAddressServices, AddressServices>();

            Services.AddScoped<IDishRepository, DishRepository>();
            Services.AddScoped<IDriverRepository, DriverRepository>();
            Services.AddScoped<IOrderRepository, OrderRepository>();
            Services.AddScoped<IOrderDishRepository, OrderDishRepository>();
            Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
            Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
            Services.AddScoped<IAccountRepository, AccountRepository>();
            Services.AddScoped<IDeliveryDetailsRepository, DeliveryDetailsRepository>();
            Services.AddScoped<IAccountRepository, AccountRepository>();
            Services.AddScoped<IAddressRepository, AddressRepository>();
            Services.AddScoped<IPaymentRepository, PaymentRepository>();
            Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            Services.AddScoped<IRefreshTokenService, RefreshTokenService>();






            return Services;
        }
    }
}
