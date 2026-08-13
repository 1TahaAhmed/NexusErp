using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Application.Common.Models.Payment;

namespace NexusErp.Infrastructure.Services.Payment;

public class PaymobService : IPaymentGatewayService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public PaymobService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public string ProviderName => "Paymob";

    public async Task<PaymentGatewayResult> ProcessPaymentAsync(
        PaymentGatewayRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var apiKey = _configuration["PaymobSettings:ApiKey"];
            var integrationIdStr = _configuration["PaymobSettings:IntegrationId"];
            var iframeId = _configuration["PaymobSettings:IframeId"];

            if (string.IsNullOrWhiteSpace(apiKey) || !int.TryParse(integrationIdStr, out var integrationId))
            {
                return new PaymentGatewayResult(false, "", ProviderName, ErrorMessage: "Paymob settings are incomplete in appsettings.");
            }

            var authResponse = await _httpClient.PostAsJsonAsync("auth/tokens", new
            {
                api_key = apiKey
            }, cancellationToken);

            if (!authResponse.IsSuccessStatusCode)
                return new PaymentGatewayResult(false, "", ProviderName, ErrorMessage: "Failed to connect to the payment gateway (Auth Failed).");

            var authData = await authResponse.Content.ReadFromJsonAsync<PaymobAuthResponse>(cancellationToken: cancellationToken);

            var orderResponse = await _httpClient.PostAsJsonAsync("ecommerce/orders", new
            {
                auth_token = authData!.Token,
                delivery_needed = "false",
                amount_cents = (int)(request.Amount * 100),
                currency = string.IsNullOrWhiteSpace(request.Currency) ? "EGP" : request.Currency,
                merchant_order_id = request.OrderReference
            }, cancellationToken);

            if (!orderResponse.IsSuccessStatusCode)
                return new PaymentGatewayResult(false, "", ProviderName, ErrorMessage: "Failed to create the payment order.");

            var orderData = await orderResponse.Content.ReadFromJsonAsync<PaymobOrderResponse>(cancellationToken: cancellationToken);

            var keyResponse = await _httpClient.PostAsJsonAsync("acceptance/payment_keys", new
            {
                auth_token = authData.Token,
                amount_cents = (int)(request.Amount * 100),
                expiration = 3600,
                order_id = orderData!.Id,
                billing_data = new
                {
                    first_name = "Customer",
                    last_name = "ERP",
                    email = string.IsNullOrWhiteSpace(request.CustomerEmail) ? "customer@nexuserp.com" : request.CustomerEmail,
                    phone_number = "01000000000",
                    floor = "NA",
                    building = "NA",
                    street = "NA",
                    city = "Cairo",
                    state = "Cairo",
                    country = "EG"
                },
                currency = string.IsNullOrWhiteSpace(request.Currency) ? "EGP" : request.Currency,
                integration_id = integrationId
            }, cancellationToken);

            if (!keyResponse.IsSuccessStatusCode)
                return new PaymentGatewayResult(false, "", ProviderName, ErrorMessage: "Failed to retrieve the payment key (Payment Key Failed).");

            var keyData = await keyResponse.Content.ReadFromJsonAsync<PaymobPaymentKeyResponse>(cancellationToken: cancellationToken);

            var paymentTokenOrUrl = string.IsNullOrWhiteSpace(iframeId)
                ? keyData!.Token
                : $"https://accept.paymob.com/api/acceptance/post_pay/{iframeId}/{keyData!.Token}";

            return new PaymentGatewayResult(
                IsSuccess: true,
                TransactionReference: orderData.Id.ToString(),
                GatewayProvider: ProviderName,
                PaymentToken: paymentTokenOrUrl
            );
        }
        catch (Exception ex)
        {
            return new PaymentGatewayResult(false, "", ProviderName, ErrorMessage: $"Error while processing the payment: {ex.Message}");
        }
    }
}

internal record PaymobAuthResponse(
    [property: JsonPropertyName("token")] string Token
);

internal record PaymobOrderResponse(
    [property: JsonPropertyName("id")] long Id
);

internal record PaymobPaymentKeyResponse(
    [property: JsonPropertyName("token")] string Token
);