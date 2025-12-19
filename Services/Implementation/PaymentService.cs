using Domin.Enum;
using Domin.Helper;
using Domin.Models;
using Domin.paymentclasses;
using Microsoft.Extensions.Options;
using Repository.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class PaymentService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly PaymobSettings _settings;
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IHttpClientFactory httpClientFactory, IOptions<PaymobSettings> settings, IPaymentRepository paymentRepository)
        {
            _httpClientFactory = httpClientFactory;
            _settings = settings.Value;
            _paymentRepository = paymentRepository;
        }

        private HttpClient CreateClient()
        {
            return _httpClientFactory.CreateClient("PaymobClient");
        }

        private async Task<string> GetAuthTokenAsync()
        {
            var client = CreateClient();
            var response = await client.PostAsJsonAsync(
                "api/auth/tokens",
                new { api_key = _settings.ApiKey });

            var data = await response.Content.ReadFromJsonAsync<dynamic>();
            return data.token;
        }

        private async Task<int> CreatePaymobOrderAsync(string token, decimal amount, int orderId)
        {
            var client = CreateClient();

            var orderRequest = new
            {
                auth_token = token,
                delivery_needed = false,
                amount_cents = (int)(amount * 100),
                currency = "EGP",
                merchant_order_id = orderId
            };

            var response = await client.PostAsJsonAsync("api/ecommerce/orders", orderRequest);
            var data = await response.Content.ReadFromJsonAsync<dynamic>();
            return data.id;
        }

        private async Task<string> GetPaymentKeyAsync(string token, int paymobOrderId, decimal amount, string email, string name, string phone)
        {
            var client = CreateClient();

            var billingData = new
            {
                apartment = "NA",
                email = email,
                floor = "NA",
                first_name = name,
                street = "NA",
                building = "NA",
                phone_number = phone,
                shipping_method = "NA",
                postal_code = "NA",
                city = "Cairo",
                country = "EG",
                last_name = name,
                state = "NA"
            };

            var paymentKeyRequest = new
            {
                auth_token = token,
                amount_cents = (int)(amount * 100),
                expiration = 3600,
                order_id = paymobOrderId,
                billing_data = billingData,
                currency = "EGP",
                integration_id = _settings.IntegrationId
            };

            var response = await client.PostAsJsonAsync("api/acceptance/payment_keys", paymentKeyRequest);
            var data = await response.Content.ReadFromJsonAsync<dynamic>();
            return data.token;
        }

        public async Task<string> CreatePaymentAsync(PayNowRequest request)
        {
            var token = await GetAuthTokenAsync();
            var paymobOrderId = await CreatePaymobOrderAsync(token, request.amount, request.orderId);

            var transaction = new PaymentTransaction
            {
                Amount = request.amount,
                OrderId = request.orderId,
                CreatedAt = DateTime.Now,
                Currency = "EGP",
                PaymentGateway = "Paymob",
                PaymentMethod = TypeOfPaymentMethod.CreditCard,
                PaymobOrderId = paymobOrderId,
                Status = PaymentStatus.Pending
            };

            await _paymentRepository.AddTransactionAsync(transaction);

            var paymentKey = await GetPaymentKeyAsync(token, paymobOrderId, request.amount, request.email, request.name, request.phone);
            var iframeUrl = $"https://accept.paymob.com/api/acceptance/iframes/{_settings.IframeId}?payment_token={paymentKey}";

            return iframeUrl;
        }
    }
}
