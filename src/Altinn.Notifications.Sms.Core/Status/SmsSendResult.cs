﻿namespace Altinn.Notifications.Sms.Core.Status;

/// <summary>
/// Enum describing sms send result types
/// </summary>
public enum SmsSendResult
{
    /// <summary>
    /// Sms send operation running
    /// </summary>
    Sending,

    /// <summary>
    /// Sms send operation accepted
    /// </summary>
    Accepted,

    /// <summary>
    /// Sms send operation failed
    /// </summary>
    Failed,

    /// <summary>
    /// Sms send operation failed due to invalid receiver
    /// </summary>
    Failed_InvalidReceiver,

    /// <summary>
    /// No information of delivery status available.
    /// </summary>
    UNKNOWN,

    /// <summary>
    /// Message was successfully delivered to destination.
    /// </summary>
    DELIVERED,

    /// <summary>
    /// Message validity period has expired.
    /// </summary>
    EXPIRED,

    /// <summary>
    /// Message has been deleted.
    /// </summary>
    DELETED,

    /// <summary>
    /// The SMS was undeliverable (not a valid number or no available route to destination).
    /// </summary>
    UNDELIVERED,

    /// <summary>
    /// Message was rejected.
    /// </summary>
    REJECTED,

    /// <summary>
    /// The SMS failed to be delivered because no operator accepted the message or due to internal Gateway error.
    /// </summary>
    FAILED,

    /// <summary>
    /// No delivery report received from operator. Unknown delivery status.
    /// </summary>
    NULL,

    /// <summary>
    /// The receiver number is barred/blocked/not in use. Do not retry message, and remove number from any subscriber list.
    /// </summary>
    BARRED
}
