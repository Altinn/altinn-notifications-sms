namespace Altinn.Notifications.Sms.Core.OneTimePassword;

/// <summary>
/// Represents a domain model for sending a one-time password (OTP) via SMS.
/// </summary>
public record OneTimePasswordPayload
{
    /// <summary>
    /// The message content containing the one-time password to be sent.
    /// </summary>
    public required string Message { get; init; }

    /// <summary>
    /// The unique identifier of the SMS notification.
    /// </summary>
    public required Guid NotificationId { get; init; }

    /// <summary>
    /// The recipient's phone number.
    /// </summary>
    public required string Recipient { get; init; }

    /// <summary>
    /// The sender of the SMS, typically the service name, phone number, or identifier.
    /// </summary>
    public required string Sender { get; init; }
}
