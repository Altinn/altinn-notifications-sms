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
}
