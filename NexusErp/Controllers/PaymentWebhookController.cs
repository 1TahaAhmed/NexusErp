using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NexusErp.Application.Common.Models.Payments;
using NexusErp.Application.Payments.Commands;
using System.Security.Cryptography;
using System.Text;

namespace NexusErp.Api.Controllers;

[ApiController]
[Route("api/payments/webhooks")]
public class PaymobWebhookController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IMediator _mediator;

    public PaymobWebhookController(IConfiguration configuration, IMediator mediator)
    {
        _configuration = configuration;
        _mediator = mediator;
    }
    
    [HttpPost("paymob")]
    public async Task<IActionResult> HandlePaymobCallback([FromBody] PaymobCallbackDto callback)
    {
        var hmacSecret = _configuration["PaymobSettings:HmacSecret"];

        if (string.IsNullOrEmpty(hmacSecret))
        {
            return BadRequest("HMAC Secret is not configured.");
        }

        if (callback?.Obj == null || callback.Obj.Order == null || callback.Obj.SourceData == null)
        {
            return BadRequest("Invalid payload structure.");
        }

        string concatenatedData = $"{callback.Obj.AmountCents}" +
                                 $"{callback.Obj.CreatedAt}" +
                                 $"{callback.Obj.Currency}" +
                                 $"{callback.Obj.ErrorOccured.ToString().ToLower()}" +
                                 $"{callback.Obj.HasParentTransaction.ToString().ToLower()}" +
                                 $"{callback.Obj.Id}" +
                                 $"{callback.Obj.IntegrationId}" +
                                 $"{callback.Obj.Is3dSecure.ToString().ToLower()}" +
                                 $"{callback.Obj.IsAuth.ToString().ToLower()}" +
                                 $"{callback.Obj.IsCapture.ToString().ToLower()}" +
                                 $"{callback.Obj.IsRefunded.ToString().ToLower()}" +
                                 $"{callback.Obj.IsStandalonePayment.ToString().ToLower()}" +
                                 $"{callback.Obj.Pending.ToString().ToLower()}" +
                                 $"{callback.Obj.Order.Id}" +
                                 $"{callback.Obj.Owner}" +
                                 $"{callback.Obj.SourceData.Pan}" +
                                 $"{callback.Obj.SourceData.SubType}" +
                                 $"{callback.Obj.SourceData.Type}" +
                                 $"{callback.Obj.Success.ToString().ToLower()}";

        using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(hmacSecret));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(concatenatedData));
        var calculatedHmac = BitConverter.ToString(hash).Replace("-", "").ToLower();

        if (calculatedHmac != callback.Hmac?.ToLower())
        {
            return BadRequest(new { message = "Invalid HMAC Signature" });
        }

        if (callback.Obj.Success)
        {
            if (!Guid.TryParse(callback.Obj.Order.MerchantOrderId, out Guid saleInvoiceId))
            {
                return BadRequest(new { error = "Invalid Guid Format", value = callback.Obj.Order.MerchantOrderId });
            }

            var result = await _mediator.Send(new ProcessPaymentSuccessCommand(saleInvoiceId, callback.Obj.Id));

            if (!result)
            {
                return NotFound(new { error = "Invoice not found in DB", searchedGuid = saleInvoiceId });
            }

            return Ok(new { status = "Payment Processed Successfully", orderId = callback.Obj.Order.MerchantOrderId });
        }

        return Ok(new { status = "Payment Failed" });
    }
}