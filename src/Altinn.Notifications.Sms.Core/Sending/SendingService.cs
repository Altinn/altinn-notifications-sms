using Altinn.Notifications.Sms.Core.Configuration;
using Altinn.Notifications.Sms.Core.Dependencies;
using Altinn.Notifications.Sms.Core.Shared;
using Altinn.Notifications.Sms.Core.Status;

namespace Altinn.Notifications.Sms.Core.Sending;

/// <summary>
/// Service responsible for sending SMS messages.
/// </summary>
public class SendingService : ISendingService
{
    private readonly ISmsClient _smsClient;
    private readonly TopicSettings _settings;
    private readonly ICommonProducer _producer;

    /// <summary>
    /// Initializes a new instance of the <see cref="SendingService"/> class.
    /// </summary>
    public SendingService(ISmsClient smsClient, ICommonProducer producer, TopicSettings settings)
    {
        _producer = producer;
        _settings = settings;
        _smsClient = smsClient;
    }

    /// <inheritdoc/>
    public async Task SendAsync(Sms sms)
    {
        await ProcessSendResult(sms, await _smsClient.SendAsync(sms));
    }

    /// <inheritdoc/>
    public async Task SendAsync(Sms sms, int timeToLiveInSeconds)
    {
        await ProcessSendResult(sms, await _smsClient.SendAsync(sms, timeToLiveInSeconds));
    }

    /// <summary>
    /// Processes the result of the SMS send operation by updating the operation result and publishing the status update.
    /// </summary>
    /// <param name="sms">The SMS message that was attempted to be sent.</param>
    /// <param name="result">The result of the SMS send operation, containing either a gateway reference or an error response.</param>
    private async Task ProcessSendResult(Sms sms, Result<string, SmsClientErrorResponse> result)
    {
        var operationResult = new SendOperationResult
        {
            NotificationId = sms.NotificationId
        };

        await result.Match(
            async gatewayReference =>
            {
                operationResult.GatewayReference = gatewayReference;
                operationResult.SendResult = SmsSendResult.Accepted;

                await PublishStatusUpdate(operationResult);
            },
            async smsSendFailResponse =>
            {
                operationResult.GatewayReference = string.Empty;
                operationResult.SendResult = smsSendFailResponse.SendResult;

                await PublishStatusUpdate(operationResult);
            });
    }

    /// <summary>
    /// Publishes the status update for the SMS send operation to the configured topic.
    /// </summary>
    /// <param name="operationResult">The result of the SMS send operation to be published.</param>
    private async Task PublishStatusUpdate(SendOperationResult operationResult)
    {
        await _producer.ProduceAsync(_settings.SmsStatusUpdatedTopicName, operationResult.Serialize());
    }
}
