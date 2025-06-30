using Altinn.Notifications.Sms.Core.Configuration;
using Altinn.Notifications.Sms.Core.Dependencies;
using Altinn.Notifications.Sms.Core.Shared;
using Altinn.Notifications.Sms.Core.Status;

namespace Altinn.Notifications.Sms.Core.Sending;

/// <summary>
/// Service responsible for handling sms sending requests.
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
        Result<string, SmsClientErrorResponse> result = await _smsClient.SendAsync(sms);

        SendOperationResult operationResult = new()
        {
            NotificationId = sms.NotificationId,
        };

        await result.Match(
            async gatewayReference =>
            {
                operationResult.GatewayReference = gatewayReference;
                operationResult.SendResult = SmsSendResult.Accepted;

                await _producer.ProduceAsync(_settings.SmsStatusUpdatedTopicName, operationResult.Serialize());
            },
            async smsSendFailResponse =>
            {
                operationResult.GatewayReference = string.Empty;
                operationResult.SendResult = smsSendFailResponse.SendResult;

                await _producer.ProduceAsync(_settings.SmsStatusUpdatedTopicName, operationResult.Serialize());
            });
    }
}
