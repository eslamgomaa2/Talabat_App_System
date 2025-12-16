using Domin.Helper;
using Domin.paymentclasses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class PaymentGetwayServices : IPaymentGetway
    {
        private readonly HttpClient _httpClient;
        private readonly PaymobSettings _settings;

        public PaymentGetwayServices(HttpClient httpClient, IOptions<PaymobSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public async Task<string> GetAuthTokenAsync()
        {
            var request = new AuthTokenRequest { ApiKey = _settings.ApiKey };
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_settings.BaseUrl}/auth/tokens", content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var authResponse = JsonConvert.DeserializeObject<AuthTokenResponce>(responseContent)??throw new Exception("faild to get token");
            return authResponse.token;
        }

        // Uses: OrderRequest, OrderResponse
        public async Task<OrderResponce> CreateOrderAsync(decimal amount, string currency = "EGP")
        {
            var authToken = await GetAuthTokenAsync();
            var request = new Orderrequest
            {
                AuthToken = authToken,
                AmountCents = ((int)(amount * 100)).ToString(),
                Currency = currency
            };

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_settings.BaseUrl}/ecommerce/orders", content);

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<OrderResponce>(responseContent);
        }

        // ===== PAYMENT KEY GENERATION =====
        // Uses: PaymentKeyRequest, PaymentKeyResponse, BillingData
        public async Task<string> GetPaymentKeyAsync(int orderId, decimal amount, BillingData billingData, string currency = "EGP")
        {
            var authToken = await GetAuthTokenAsync();
            var request = new PaymentKeyRequest
            {
                AuthToken = authToken,
                Amount = amount ,
                OrderId = orderId.ToString(),
                BillingData = billingData, // ← BillingData DTO used here
                Currency = currency,
                IntegrationId = int.Parse(_settings.IntegrationId)
            };

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_settings.BaseUrl}/acceptance/payment_keys", content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var paymentKeyResponse = JsonConvert.DeserializeObject<PaymentKeyResponse>(responseContent);
            return paymentKeyResponse.token;
        }

        
        public bool ValidateCallback(PaymentCallbackData callbackData, string hmacHeader)
        {
            // All properties of PaymentCallbackData and its nested objects are used for HMAC validation
            var concatenatedString = $"{callbackData.Amount_cents}" +
                                   $"{callbackData.Created_at:yyyy-MM-dd\\THH:mm:ss.ffffff}" +
                                   $"{callbackData.Currency}" +
                                   $"{callbackData.Error_occured}" +
                                   $"{callbackData.Has_parent_transaction}" +
                                   $"{callbackData.Id}" +
                                   $"{callbackData.Integration_id}" +
                                   $"{callbackData.Is_3d_secure}" +
                                   $"{callbackData.Is_auth}" +
                                   $"{callbackData.Is_capture}" +
                                   $"{callbackData.Is_refunded}" +
                                   $"{callbackData.Is_standalone_payment}" +
                                   $"{callbackData.Is_voided}" +
                                   // Using nested PaymentOrder DTO
                                   $"{callbackData.Order.Id}" +
                                   $"{callbackData.Owner}" +
                                   $"{callbackData.Pending}" +
                                   // Using nested PaymentSourceData DTO
                                   $"{callbackData.Source_data?.Pan}" +
                                   $"{callbackData.Source_data?.Type}" +
                                   $"{callbackData.Success}";

            var hmacHash = ComputeHmacSha512(_settings.HmacHash, concatenatedString);
            return hmacHash.Equals(hmacHeader, StringComparison.OrdinalIgnoreCase);
        }

        

        private string ComputeHmacSha512(string key, string data)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(Encoding.UTF8.GetBytes(key)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return Convert.ToHexString(hash).ToLower();
            }
        }

        public string GenerateIframeUrl(string paymentToken)
        {
            return $"https://accept.paymob.com/api/acceptance/iframes/{_settings.IframeId}?payment_token={paymentToken}";
        }
    }

}
