namespace Altinn.Notifications.Sms.Core.OneTimePassword;

/// <summary>
/// Represents a domain model for sending a one-time password (OTP) via SMS.
/// </summary>
public record OneTimePasswordOutcome
{
    /// <summary>
    /// Indicates whether the SMS was accepted for delivery.
    /// </summary>
    /// <value>
    /// <c>true</c> if the SMS was accepted for delivery; otherwise, <c>false</c>.
    /// </value>
    public required bool IsAccepted { get; init; }

    /// <summary>
    /// The unique identifier of the SMS notification.
    /// </summary>
    public required Guid NotificationId { get; init; }
}
