using System.Text.Json;

using Altinn.Notifications.Sms.Core.Configuration;
using Altinn.Notifications.Sms.Core.Dependencies;

using LinkMobility.PSWin.Receiver.Model;

using Microsoft.Extensions.Logging;

namespace Altinn.Notifications.Sms.Core.Status;

/// <summary>
/// Service for handling status updates
/// </summary>
public class StatusService : IStatusService
{
    private readonly TopicSettings _settings;
    private readonly ICommonProducer _producer;
    private readonly ILogger<IStatusService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="StatusService"/> class.
    /// </summary>
    public StatusService(ICommonProducer producer, TopicSettings settings, ILogger<IStatusService> logger)
    {
        _settings = settings;
        _producer = producer;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task UpdateStatusAsync(DrMessage message)
    {
        _logger.LogInformation("// StatusService // UpdateStatusAsync // DR received {deliveryMessage}", JsonSerializer.Serialize(message));
        SendOperationResult result = new()
        {
            GatewayReference = message.Reference,
            SendResult = SmsSendResultMapper.ParseDeliveryState(message.State)
        };
        await _producer.ProduceAsync(_settings.SmsStatusUpdatedTopicName, result.Serialize());
    }
}
