using NexusErp.Application.Common.Models.Payment;

namespace NexusErp.Application.Common.Interfaces;

public interface IPaymentGatewayService
{
    string ProviderName { get; }
    Task<PaymentGatewayResult> ProcessPaymentAsync(
        PaymentGatewayRequest request,
        CancellationToken cancellationToken = default);
}