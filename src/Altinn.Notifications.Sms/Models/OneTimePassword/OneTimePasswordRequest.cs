using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Altinn.Notifications.Sms.Models.OneTimePassword;

/// <summary>
/// Represents a request model for sending a one-time password via SMS.
/// </summary>
public record OneTimePasswordRequest
{
    /// <summary>
    /// The message that contains the one-time password.
    /// </summary>
    [Required]
    [JsonPropertyName("message")]
    public required string Message { get; init; }

    /// <summary>
    /// The unique identifier that represents this SMS notification order.
    /// </summary>
    [Required]
    [JsonPropertyName("notificationId")]
    public required Guid NotificationId { get; init; }

    /// <summary>
    /// The phone number to which the message will be sent.
    /// </summary>
    [Phone]
    [Required]
    [JsonPropertyName("recipient")]
    public required string Recipient { get; init; }

    /// <summary>
    /// The sender of the SMS.
    /// This is typically the name, phone number or identifier of the service sending the SMS.
    /// </summary>
    [Required]
    [JsonPropertyName("sender")]
    public required string Sender { get; init; }
}
