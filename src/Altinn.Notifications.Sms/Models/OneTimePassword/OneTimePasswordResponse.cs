using System.Text.Json.Serialization;

namespace Altinn.Notifications.Sms.Models.OneTimePassword;

/// <summary>
/// Represents a response model for sending a one-time password (OTP) via SMS.
/// </summary>
public record OneTimePasswordResponse
{
    /// <summary>
    /// The unique reference or identifier assigned by the SMS gateway.
    /// </summary>
    [JsonPropertyName("gatewayReference")]
    public required string? GatewayReference { get; init; }

    /// <summary>
    /// The unique identifier of the SMS notification.
    /// </summary>
    [JsonPropertyName("notificationId")]
    public required Guid NotificationId { get; init; }
}
