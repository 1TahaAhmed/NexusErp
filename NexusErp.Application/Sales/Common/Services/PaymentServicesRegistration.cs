using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Infrastructure.Services.Payment;
using NexusErp.Infrastructure.Services;       
namespace NexusErp.Infrastructure.Common.Services;

public static class PaymentServicesRegistration
{
    public static IServiceCollection AddPaymentServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IPaymentGatewayService, PaymobService>(client =>
        {
            var baseUrl = configuration["PaymobSettings:BaseUrl"] ?? "https://accept.paymob.com/api/";
            client.BaseAddress = new Uri(baseUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        return services;
    }
}