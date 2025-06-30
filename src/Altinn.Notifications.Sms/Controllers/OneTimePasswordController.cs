using Altinn.Notifications.Sms.Core.Sending;
using Altinn.Notifications.Sms.Mappers;
using Altinn.Notifications.Sms.Models.OneTimePassword;

using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

namespace Altinn.Notifications.Sms.Controllers;

/// <summary>
/// Controller for sending one-time passwords via SMS.
/// </summary>
[ApiController]
[Route("notifications/sms/api/v1/otp")]
[ApiExplorerSettings(IgnoreApi = true)]
public class OneTimePasswordController : ControllerBase
{
    private readonly ISendingService _sendingService;

    /// <summary>
    /// Initializes a new instance of the <see cref="OneTimePasswordController"/> class.
    /// </summary>
    public OneTimePasswordController(ISendingService sendingService)
    {
        _sendingService = sendingService;
    }

    /// <summary>
    /// Sends a one-time password (OTP) via SMS to the specified recipient.
    /// </summary>
    /// <param name="request">
    /// The <see cref="OneTimePasswordRequest"/> containing the OTP, recipient phone number, sender identity, and notification ID.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> that can be used to cancel the operation before completion.
    /// </param>
    /// <returns>
    /// Returns 200 (OK) when the SMS was successfully accepted by the service provider.
    /// Returns 400 (Bad Request) with <see cref="ProblemDetails"/> when the request is invalid or contains improper formatting.
    /// Returns 499 (Client Closed Request) with <see cref="ProblemDetails"/> when the client cancels the request before completion.
    /// </returns>
    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    [SwaggerResponse(200, "The SMS was accepted by the service provider.")]
    [SwaggerResponse(400, "The request was invalid.", typeof(ProblemDetails))]
    [SwaggerResponse(499, "The request was canceled before processing could complete.", typeof(ProblemDetails))]
    public async Task<ActionResult> Send([FromBody] OneTimePasswordRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var sms = request.ToSms();

            await _sendingService.SendAsync(sms);

            return Ok();
        }
        catch (InvalidOperationException)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid SMS request",
                Status = StatusCodes.Status400BadRequest,
                Detail = "The request could not be processed due to invalid input or state."
            });
        }
        catch (OperationCanceledException)
        {
            return StatusCode(StatusCodes.Status499ClientClosedRequest, new ProblemDetails
            {
                Title = "Request terminated",
                Status = StatusCodes.Status499ClientClosedRequest,
                Detail = "The request was canceled before processing could complete",
            });
        }
    }
}
