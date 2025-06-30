using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Altinn.Notifications.Sms.Models.OneTimePassword;

/// <summary>
/// Represents a request model for sending a one-time password (OTP) via SMS.
/// </summary>
public record OneTimePasswordRequest
{
    /// <summary>
    /// The message content containing the one-time password.
    /// </summary>
    [Required]
    [JsonPropertyName("message")]
    public required string Message { get; init; }

    /// <summary>
    /// The unique identifier of the SMS notification.
    /// </summary>
    [Required]
    [JsonPropertyName("notificationId")]
    public required Guid NotificationId { get; init; }

    /// <summary>
    /// The recipient's phone number.
    /// </summary>
    [Required]
    [JsonPropertyName("recipient")]
    public required string Recipient { get; init; }

    /// <summary>
    /// The sender of the SMS, typically the service name, phone number, or identifier.
    /// </summary>
    [Required]
    [JsonPropertyName("sender")]
    public required string Sender { get; init; }
}
