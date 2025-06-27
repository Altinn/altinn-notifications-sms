using System.Text.Json.Serialization;

namespace Altinn.Notifications.Sms.Models.OneTimePassword;

/// <summary>
/// Represents a response model for sending a one-time password via SMS.
/// </summary>
public record OneTimePasswordResponse
{
    /// <summary>
    /// Indicates whether the SMS was accepted by the service provider.
    /// </summary>
    /// <value>
    /// <c>true</c> if the SMS was accepted by the service provider; otherwise, <c>false</c>.
    /// </value>
    [JsonPropertyName("isAccepted")]
    public required bool IsAccepted { get; init; }

    /// <summary>
    /// The unique identifier that represents this SMS notification order.
    /// </summary>
    [JsonPropertyName("notificationId")]
    public required Guid NotificationId { get; init; }
}
