using Altinn.Notifications.Sms.Models.OneTimePassword;

namespace Altinn.Notifications.Sms.Mappers;

/// <summary>
/// Provides mapping methods between API models and domain models for one-time password (OTP) SMS notifications.
/// </summary>
public static class OneTimePasswordMapper
{
    /// <summary>
    /// Maps a <see cref="OneTimePasswordRequest"/> API model to a <see cref="Core.Sending.Sms"/> domain model.
    /// </summary>
    /// <param name="request">The API request model to map from.</param>
    /// <returns>A domain payload model with the mapped data.</returns>
    public static Core.Sending.Sms ToSms(this OneTimePasswordRequest request)
    {
        return new Core.Sending.Sms
        {
            Sender = request.Sender,
            Message = request.Message,
            Recipient = request.Recipient,
            NotificationId = request.NotificationId
        };
    }
}
