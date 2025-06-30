namespace Altinn.Notifications.Sms.Core.OneTimePassword;

/// <summary>
/// Represents a domain model for sending a one-time password (OTP) via SMS.
/// </summary>
public record OneTimePasswordOutcome
{
    /// <summary>
    /// The unique reference or identifier assigned by the SMS gateway.
    /// </summary>
    public required string? GatewayReference { get; init; }

    /// <summary>
    /// The unique identifier of the SMS notification.
    /// </summary>
    public required Guid NotificationId { get; init; }
}
