namespace NexusErp.Application.Common.Models.Payment;

public record PaymentGatewayRequest(
    decimal Amount,
    string Currency,
    string CustomerEmail,
    string OrderReference
);

public record PaymentGatewayResult(
    bool IsSuccess,
    string TransactionReference,
    string GatewayProvider,
    string PaymentToken = "",
    string ErrorMessage = ""
);