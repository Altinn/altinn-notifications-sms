using Altinn.Notifications.Sms.Core.Status;
using LinkMobility.PSWin.Receiver.Model;

namespace Altinn.Notifications.Sms.Tests.Sms.Core.Status;

public class SmsSendResultTests
{
    [Theory]
    [InlineData(DeliveryState.UNKNOWN, SmsSendResult.Unknown)]
    [InlineData(DeliveryState.DELIVRD, SmsSendResult.Delivered)]
    [InlineData(DeliveryState.EXPIRED, SmsSendResult.Expired)]
    [InlineData(DeliveryState.DELETED, SmsSendResult.Deleted)]
    [InlineData(DeliveryState.UNDELIV, SmsSendResult.Undelivered)]
    [InlineData(DeliveryState.REJECTD, SmsSendResult.Rejected)]
    [InlineData(DeliveryState.FAILED, SmsSendResult.Failed)]
    [InlineData(DeliveryState.NULL, SmsSendResult.Null)]
    [InlineData(DeliveryState.BARRED, SmsSendResult.Barred)]
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
