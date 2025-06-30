using Altinn.Notifications.Sms.Core.OneTimePassword;
using Altinn.Notifications.Sms.Models.OneTimePassword;

namespace Altinn.Notifications.Sms.Mappers;

/// <summary>
/// Provides mapping methods between API models and domain models for one-time password (OTP) SMS notifications.
/// </summary>
public static class OneTimePasswordMapper
{
    /// <summary>
    /// Maps a <see cref="OneTimePasswordRequest"/> API model to a <see cref="OneTimePasswordPayload"/> domain model.
    /// </summary>
    /// <param name="request">The API request model to map from.</param>
    /// <returns>A domain payload model with the mapped data.</returns>
    public static OneTimePasswordPayload ToPayload(this OneTimePasswordRequest request)
    {
        return new OneTimePasswordPayload
        {
            Sender = request.Sender,
            Message = request.Message,
            Recipient = request.Recipient,
            NotificationId = request.NotificationId
        };
    }

    /// <summary>
    /// Maps a <see cref="OneTimePasswordOutcome"/> domain model to a <see cref="OneTimePasswordResponse"/> API model.
    /// </summary>
    /// <param name="outcome">The domain outcome model to map from.</param>
    /// <returns>An API response model with the mapped data.</returns>
    public static OneTimePasswordResponse ToResponse(this OneTimePasswordOutcome outcome)
    {
        return new OneTimePasswordResponse
        {
            NotificationId = outcome.NotificationId,
            GatewayReference = outcome.GatewayReference
        };
    }
}
