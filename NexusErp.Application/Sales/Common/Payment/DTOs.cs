using System.Text.Json.Serialization;

namespace NexusErp.Application.Common.Models.Payments;

public class PaymobCallbackDto
{
    [JsonPropertyName("hmac")]
    public string Hmac { get; set; } = string.Empty;

    [JsonPropertyName("obj")]
    public PaymobCallbackObj Obj { get; set; } = new();
}

public class PaymobCallbackObj
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("amount_cents")]
    public int AmountCents { get; set; }

    [JsonPropertyName("created_at")]
    public string CreatedAt { get; set; } = string.Empty;

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;

    [JsonPropertyName("error_occured")]
    public bool ErrorOccured { get; set; }

    [JsonPropertyName("has_parent_transaction")]
    public bool HasParentTransaction { get; set; }

    [JsonPropertyName("integration_id")]
    public int IntegrationId { get; set; }

    [JsonPropertyName("is_3d_secure")]
    public bool Is3dSecure { get; set; }

    [JsonPropertyName("is_auth")]
    public bool IsAuth { get; set; }

    [JsonPropertyName("is_capture")]
    public bool IsCapture { get; set; }

    [JsonPropertyName("is_refunded")]
    public bool IsRefunded { get; set; }

    [JsonPropertyName("is_standalone_payment")]
    public bool IsStandalonePayment { get; set; }

    [JsonPropertyName("pending")]
    public bool Pending { get; set; }

    [JsonPropertyName("order")]
    public PaymobOrderData Order { get; set; } = new();

    [JsonPropertyName("owner")]
    public int Owner { get; set; }

    [JsonPropertyName("source_data")]
    public PaymobSourceData SourceData { get; set; } = new();
}

public class PaymobOrderData
{
    [JsonPropertyName("merchant_order_id")]
    public string MerchantOrderId { get; set; } = string.Empty;

    [JsonPropertyName("id")]
    public long Id { get; set; }
}

public class PaymobSourceData
{
    [JsonPropertyName("pan")]
    public string Pan { get; set; } = string.Empty;

    [JsonPropertyName("sub_type")]
    public string SubType { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
}