using Altinn.Notifications.Sms.Core.Status;
using LinkMobility.PSWin.Receiver.Model;

namespace Altinn.Notifications.Sms.Tests.Sms.Core.Status;

public class SmsSendResultTests
{
    [Theory]
    [InlineData(DeliveryState.UNKNOWN, SmsSendResult.UNKNOWN)]
    [InlineData(DeliveryState.DELIVRD, SmsSendResult.DELIVERED)]
    [InlineData(DeliveryState.EXPIRED, SmsSendResult.EXPIRED)]
    [InlineData(DeliveryState.DELETED, SmsSendResult.DELETED)]
    [InlineData(DeliveryState.UNDELIV, SmsSendResult.UNDELIVERED)]
    [InlineData(DeliveryState.REJECTD, SmsSendResult.REJECTED)]
    [InlineData(DeliveryState.FAILED, SmsSendResult.FAILED)]
    [InlineData(DeliveryState.NULL, SmsSendResult.NULL)]
    [InlineData(DeliveryState.BARRED, SmsSendResult.BARRED)]
    public void ParseDeliveryState_WithValidInput_ReturnsExpectedResult(DeliveryState input, SmsSendResult expected)
    {
        // Act
        var result = SmsSendResultMapper.ParseDeliveryState(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ParseDeliveryState_WithUnhandledState_ThrowsArgumentException()
    {
        // Arrange
        var unhandledState = (DeliveryState)999;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => SmsSendResultMapper.ParseDeliveryState(unhandledState));
    }
}
