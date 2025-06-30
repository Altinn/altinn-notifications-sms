using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

using Altinn.Notifications.Sms.Core.OneTimePassword;
using Altinn.Notifications.Sms.Core.Sending;
using Altinn.Notifications.Sms.Models.OneTimePassword;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

using Moq;

namespace Altinn.Notifications.Sms.IntegrationTests.OneTimePasswordController;

public class OneTimePasswordControllerTests : IClassFixture<IntegrationTestWebApplicationFactory<Controllers.OneTimePasswordController>>
{
    private readonly IntegrationTestWebApplicationFactory<Controllers.OneTimePasswordController> _factory;
    private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public OneTimePasswordControllerTests(IntegrationTestWebApplicationFactory<Controllers.OneTimePasswordController> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Send_WithValidRequest_ReturnsOkAndPopulatedResponse()
    {
        // Arrange
        var notificationId = Guid.NewGuid();
        var gatewayReference = "F5F70413-218F-4BD0-AC0A-CFF0BE9A8662";

        var sendingServiceMock = new Mock<ISendingService>();
        sendingServiceMock
            .Setup(e => e.SendAsync(It.Is<OneTimePasswordPayload>(e => e.NotificationId == notificationId)))
            .ReturnsAsync((OneTimePasswordPayload payload) =>
                new OneTimePasswordOutcome
                {
                    GatewayReference = gatewayReference,
                    NotificationId = payload.NotificationId,
                });

        var httpClient = GetTestClient(sendingServiceMock.Object);
        var oneTimePasswordRequest = new OneTimePasswordRequest
        {
            Sender = "TestService",
            Recipient = "+4799999999",
            NotificationId = notificationId,
            Message = "Your one time password is: 2A31519EC7C6",
        };

        // Act
        var response = await httpClient.PostAsJsonAsync("/notifications/sms/api/v1/otp", oneTimePasswordRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseBody = await response.Content.ReadAsStringAsync();
        var sendingResponse = JsonSerializer.Deserialize<OneTimePasswordResponse>(responseBody, _jsonOptions);

        Assert.NotNull(sendingResponse);
        Assert.Equal(gatewayReference, sendingResponse.GatewayReference);
        Assert.Equal(oneTimePasswordRequest.NotificationId, sendingResponse.NotificationId);
    }

    [Fact]
    public async Task Send_WithMissingOrInvalidFields_ReturnsBadRequestWithValidationError()
    {
        // Arrange
        var sendingServiceMock = new Mock<ISendingService>();
        var httpClient = GetTestClient(sendingServiceMock.Object);
        var invalidJson = """
        {
            "sender": "",
            "message": "",
            "recipient": "",
            "notificationId": "00000000-0000-0000-0000-000000000000"
        }
        """;

        var content = new StringContent(invalidJson, System.Text.Encoding.UTF8, "application/json");

        // Act
        var response = await httpClient.PostAsync("/notifications/sms/api/v1/otp", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var responseBody = await response.Content.ReadAsStringAsync();
        var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(responseBody, _jsonOptions);

        Assert.NotNull(problemDetails);
        Assert.Equal(400, problemDetails.Status);
        Assert.Equal("One or more validation errors occurred.", problemDetails.Title);
    }

    [Fact]
    public async Task Send_WithNullGatewayReference_ReturnsBadRequest()
    {
        // Arrange
        var notificationId = Guid.NewGuid();

        var sendingServiceMock = new Mock<ISendingService>();
        sendingServiceMock
            .Setup(e => e.SendAsync(It.IsAny<OneTimePasswordPayload>()))
            .ReturnsAsync(new OneTimePasswordOutcome
            {
                GatewayReference = null,
                NotificationId = notificationId,
            });

        var httpClient = GetTestClient(sendingServiceMock.Object);
        var oneTimePasswordRequest = new OneTimePasswordRequest
        {
            Sender = "TestService",
            Recipient = "+4799999999",
            NotificationId = notificationId,
            Message = "Your one time password is: 2A31519EC7C6",
        };

        // Act
        var response = await httpClient.PostAsJsonAsync("/notifications/sms/api/v1/otp", oneTimePasswordRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var responseBody = await response.Content.ReadAsStringAsync();
        var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(responseBody, _jsonOptions);

        Assert.NotNull(problemDetails);
        Assert.Equal(400, problemDetails.Status);
    }

    [Fact]
    public async Task Send_WhenRequestIsCanceled_Returns499ClientClosedRequest()
    {
        // Arrange
        using var cancellationTokenSource = new CancellationTokenSource();

        var sendingServiceMock = new Mock<ISendingService>();
        sendingServiceMock
            .Setup(s => s.SendAsync(It.IsAny<OneTimePasswordPayload>()))
            .ThrowsAsync(new OperationCanceledException());

        var httpClient = GetTestClient(sendingServiceMock.Object);

        var oneTimePasswordRequest = new OneTimePasswordRequest
        {
            Sender = "TestService",
            Message = "OTP: 088E863A",
            Recipient = "+4799999999",
            NotificationId = Guid.NewGuid(),
        };

        // Act
        var response = await httpClient.PostAsJsonAsync("/notifications/sms/api/v1/otp", oneTimePasswordRequest, cancellationTokenSource.Token);

        // Assert
        Assert.Equal((HttpStatusCode)499, response.StatusCode);

        var responseBody = await response.Content.ReadAsStringAsync();
        var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(responseBody, _jsonOptions);

        Assert.NotNull(problemDetails);
        Assert.Equal(499, problemDetails.Status);
        Assert.Contains("Request terminated", problemDetails.Title, StringComparison.OrdinalIgnoreCase);
    }

    private HttpClient GetTestClient(ISendingService sendingService)
    {
        return _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton(sendingService);
            });
        }).CreateClient();
    }
}
