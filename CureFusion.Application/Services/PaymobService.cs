using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace CureFusion.Application.Services;

public class PaymobService
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;

    public PaymobService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _config = config;
    }

    public async Task<string> GeneratePaymentLink(decimal amount, int appointmentId)
    {
        var apiKey = _config["Paymob:ApiKey"];
        var integrationId = int.Parse(_config["Paymob:IntegrationId"]);
        var iframeId = _config["Paymob:IframeId"];


        var authRes = await _http.PostAsJsonAsync("https://accept.paymob.com/api/auth/tokens", new { api_key = apiKey });
        var authJson = await authRes.Content.ReadFromJsonAsync<JsonElement>();
        var token = authJson.GetProperty("token").GetString();


        var orderRes = await _http.PostAsJsonAsync("https://accept.paymob.com/api/ecommerce/orders", new
        {
            auth_token = token,
            delivery_needed = false,
            amount_cents = (int)(amount * 100),
            merchant_order_id = $"{appointmentId.ToString()}_{DateTime.UtcNow.Ticks}",
            items = Array.Empty<object>()
        });
        var orderContent = await orderRes.Content.ReadAsStringAsync();
        var orderJson = JsonDocument.Parse(orderContent).RootElement;
        if (!orderJson.TryGetProperty("id", out var idProp))
        {
            throw new Exception("Missing 'id' in response: " + orderContent);
        }

        var orderId = idProp.GetInt32();
        var billingData = new
        {
            first_name = "Cure",
            last_name = "Fusion",
            email = "youssifelish@outlook.com",
            phone_number = "01093441321",
            country = "EG",
            city = "Cairo",
            state = "Cairo",
            street = "Main St",
            building = "1",
            floor = "1",
            apartment = "1"
        };

        var paymentRes = await _http.PostAsJsonAsync("https://accept.paymob.com/api/acceptance/payment_keys", new
        {
            auth_token = token,
            amount_cents = (int)(amount * 100),
            expiration = 3600,
            order_id = orderId,
            billing_data = billingData,
            currency = "EGP",
            integration_id = integrationId
        });

        var paymentJson = await paymentRes.Content.ReadFromJsonAsync<JsonElement>();
        var paymentToken = paymentJson.GetProperty("token").GetString();


        return $"https://accept.paymob.com/api/acceptance/iframes/{iframeId}?payment_token={paymentToken}";
    }
}
