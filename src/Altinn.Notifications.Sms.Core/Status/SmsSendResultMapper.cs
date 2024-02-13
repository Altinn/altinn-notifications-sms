using LinkMobility.PSWin.Receiver.Model;

namespace Altinn.Notifications.Sms.Core.Status;

/// <summary>
/// Mapper handling parsing to SmsSendResult
/// </summary>
public class SmsSendResultMapper
{
    /// <summary>
    /// Parse DeliveryState to SmsSendResult 
    /// </summary>
    /// <param name="deliveryState">Delivery state from Link Mobility</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Throws exception if unknown delivery state</exception>
    public static SmsSendResult ParseDeliveryState(DeliveryState deliveryState)
    {
        return deliveryState switch
        {
            DeliveryState.UNKNOWN => SmsSendResult.UNKNOWN,
            DeliveryState.DELIVRD => SmsSendResult.DELIVERED,
            DeliveryState.EXPIRED => SmsSendResult.EXPIRED,
            DeliveryState.DELETED => SmsSendResult.DELETED,
            DeliveryState.UNDELIV => SmsSendResult.UNDELIVERED,
            DeliveryState.REJECTD => SmsSendResult.REJECTED,
            DeliveryState.FAILED => SmsSendResult.FAILED,
            DeliveryState.NULL => SmsSendResult.NULL,
            DeliveryState.BARRED => SmsSendResult.BARRED,
            _ => throw new ArgumentException($"Unhandled DeliveryState: {deliveryState}"),
        };
    }
}
