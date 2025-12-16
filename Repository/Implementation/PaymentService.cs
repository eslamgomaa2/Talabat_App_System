using Domin.Enum;
using Domin.Helper;
using Domin.Models;
using Domin.paymentclasses;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class PaymentService
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly PaymobSettings _settings;

        public PaymentService(HttpClient httpClient, IOptions<PaymobSettings> settings, ApplicationDbContext context)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _context = context;
        }


        public async Task<string> GetAuthTokenAsync()
        {
            var response = await _httpClient.PostAsJsonAsync(
                "https://accept.paymob.com/api/auth/tokens",
                new { api_key = _settings.ApiKey });

            var data = await response.Content.ReadFromJsonAsync<dynamic>();
            return data.token;
        }

        
        public async Task<int> CreatePaymobOrderAsync(string token, decimal amount, int orderId)
        {
            var orderRequest = new
            {
                auth_token = token,
                delivery_needed = false,
                amount_cents = (int)(amount * 100),
                currency = "EGP",
                merchant_order_id = orderId 
            };

            var response = await _httpClient.PostAsJsonAsync(
                "https://accept.paymob.com/api/ecommerce/orders",
                orderRequest);

            var data = await response.Content.ReadFromJsonAsync<dynamic>();
            return data.id;
        }

        
        private async Task<string> GetPaymentKeyAsync(string token, int paymobOrderId, decimal amount, string email, string name, string phone)
        {
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

            var response = await _httpClient.PostAsJsonAsync(
                "https://accept.paymob.com/api/acceptance/payment_keys",
                paymentKeyRequest);

            var data = await response.Content.ReadFromJsonAsync<dynamic>();
            return data.token;
        }

        public async Task<string> CreatePaymentAsync(PayNowRequest request)
        {
            var token = await GetAuthTokenAsync();
            var paymobOrderId = await CreatePaymobOrderAsync(token, request.amount, request.orderId);
            var transactionn = new PaymentTransaction
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
            _context.PaymentTransactions.Add(transactionn);
            await _context.SaveChangesAsync();
            var paymentKey = await GetPaymentKeyAsync(token, paymobOrderId, request.amount, request.email, request.name, request.phone);

            var iframeUrl = $"https://accept.paymob.com/api/acceptance/iframes/{_settings.IframeId}?payment_token={paymentKey}";
            return iframeUrl;
        }
    }
}

