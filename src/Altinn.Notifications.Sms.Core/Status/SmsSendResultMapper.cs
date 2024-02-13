using LinkMobility.PSWin.Receiver.Model;

namespace Altinn.Notifications.Sms.Core.Status;

/// <summary>
/// Mapper handling parsing to SmsSendResult
/// </summary>
public static class SmsSendResultMapper
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
            DeliveryState.UNKNOWN => SmsSendResult.Unknown,
            DeliveryState.DELIVRD => SmsSendResult.Delivered,
            DeliveryState.EXPIRED => SmsSendResult.Expired,
            DeliveryState.DELETED => SmsSendResult.Deleted,
            DeliveryState.UNDELIV => SmsSendResult.Undelivered,
            DeliveryState.REJECTD => SmsSendResult.Rejected,
            DeliveryState.FAILED => SmsSendResult.Failed,
            DeliveryState.NULL => SmsSendResult.Null,
            DeliveryState.BARRED => SmsSendResult.Barred,
            _ => throw new ArgumentException($"Unhandled DeliveryState: {deliveryState}"),
        };
    }
}
