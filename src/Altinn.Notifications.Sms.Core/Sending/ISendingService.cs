using Altinn.Notifications.Sms.Core.OneTimePassword;

namespace Altinn.Notifications.Sms.Core.Sending;

/// <summary>
/// Defines operations for sending SMS messages.
/// </summary>
public interface ISendingService
{
    /// <summary>
    /// Sends a standard SMS message.
    /// </summary>
    /// <param name="sms">The SMS message to be sent.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendAsync(Sms sms);

    /// <summary>
    /// Sends a one-time password (OTP) via SMS.
    /// </summary>
    /// <param name="oneTimePasswordPayload">The one-time password message to be sent.</param>
    /// <returns>
    /// A task representing the asynchronous operation with the outcome of the sending attempt.
    /// The outcome includes whether the SMS was accepted by the service provider.
    /// </returns>
    Task<OneTimePasswordOutcome> SendAsync(OneTimePasswordPayload oneTimePasswordPayload);
}
