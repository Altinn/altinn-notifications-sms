using Altinn.Notifications.Sms.Core.Sending;
using Altinn.Notifications.Sms.Mappers;
using Altinn.Notifications.Sms.Models.OneTimePassword;

using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

namespace Altinn.Notifications.Sms.Controllers
{
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
        /// The <see cref="OneTimePasswordRequest"/> containing the OTP message, recipient phone number, sender, and notification ID.
        /// The request body must be provided as JSON.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> that can be used by the client to cancel the operation before completion.
        /// </param>
        /// <returns>
        /// Returns <see cref="OneTimePasswordResponse"/> with status 200 (OK) if the SMS was accepted by the service provider.
        /// Returns <see cref="ProblemDetails"/> with status 400 (Bad Request) if the request is invalid.
        /// Returns <see cref="ProblemDetails"/> with status 499 if the request was canceled before processing could complete.
        /// </returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerResponse(400, "The request was invalid.", typeof(ProblemDetails))]
        [SwaggerResponse(200, "The SMS was accepted by the service provider.", typeof(OneTimePasswordResponse))]
        [SwaggerResponse(499, "The request was canceled before processing could complete.", typeof(ProblemDetails))]
        public async Task<ActionResult<OneTimePasswordResponse>> Send([FromBody] OneTimePasswordRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var payload = request.ToPayload();

                var outcome = await _sendingService.SendAsync(payload);

                if (string.IsNullOrWhiteSpace(outcome.GatewayReference))
                {
                    return BadRequest(new ProblemDetails
                    {
                        Title = "SMS delivery failed",
                        Status = StatusCodes.Status400BadRequest,
                        Detail = "The service provider did not accept the SMS message"
                    });
                }

                return Ok(outcome.ToResponse());
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
}
