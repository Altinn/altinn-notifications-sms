using Altinn.Notifications.Sms.Integrations.Configuration;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Altinn.Notifications.Tests.Notifications.Integrations.TestingExtensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddIntegrationServices_SmsGatewayConfig_ThrowsException()
    {
        // Arrange
        string expectedExceptionMessage = "Required SmsGatewayConfiguration settings is missing from application configuration. (Parameter 'config')";

        Environment.SetEnvironmentVariable("SmsGatewayConfiguration__Endpoint", null);

        var config = new ConfigurationBuilder().AddEnvironmentVariables().Build();

        IServiceCollection services = new ServiceCollection();

        // Act
        var exception = Assert.Throws<ArgumentNullException>(() => services.AddIntegrationServices(config));

        // Assert
        Assert.Equal(expectedExceptionMessage, exception.Message);
    }
}
