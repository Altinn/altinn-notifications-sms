using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

using Altinn.Notifications.Sms.Core.Status;
using LinkMobility.PSWin.Receiver.Model;

namespace Altinn.Notifications.Sms.Telemetry;

/// <summary>
/// Middleware that extracts delivery report data from Link Mobility SMS delivery report XML
/// in POST request bodies and adds them as tags to OpenTelemetry Activity for Application Insights tracking.
/// </summary>
/// <param name="next">The next middleware delegate in the request pipeline.</param>
public class RequestBodyTelemetryMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    /// <summary>
    /// Processes the HTTP request to extract and log SMS delivery report telemetry data.
    /// </summary>
    /// <param name="context">The HTTP context for the current request.</param>
    /// <remarks>
    /// This method reads the request body for POST requests to the delivery report endpoint,
    /// parses the XML to extract the message ID and reference, and adds them as tags to the
    /// current OpenTelemetry Activity. The request body is buffered and reset so downstream
    /// middleware and controllers can still read it.
    /// </remarks>
    public async Task InvokeAsync(HttpContext context)
    {
        // Check if it's a POST request
        if (context.Request.Method == HttpMethods.Post)
        {
            // Allow the body to be read multiple times (rewindable)
            context.Request.EnableBuffering();

            // Leave the body stream open after reading
            using var reader = new StreamReader(
                context.Request.Body,
                Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                bufferSize: 1024,
                leaveOpen: true);
            var body = await reader.ReadToEndAsync();

            // Reset the stream's position to 0 so the next middleware/controller can read it
            context.Request.Body.Position = 0;

            // Check if this is a delivery report request
            if (context.Request.Path.StartsWithSegments("/notifications/sms/api/v1/reports") && !string.IsNullOrWhiteSpace(body))
            {
                try
                {
                    // Parse the XML delivery report
                    var doc = XDocument.Parse(body);
                    var msgElement = doc.Root?.Element("MSG");
                    var id = msgElement?.Element("ID")?.Value;
                    var reference = msgElement?.Element("REF")?.Value;
                    var stateString = msgElement?.Element("STATE")?.Value;

                    // Parse the delivery state
                    if (!string.IsNullOrEmpty(stateString) && Enum.TryParse<DeliveryState>(stateString, out var deliveryState))
                    {
                        var mappedResult = SmsSendResultMapper.ParseDeliveryState(deliveryState);

                        // Add telemetry tags to the current activity
                        var activity = Activity.Current;
                        if (activity != null && !string.IsNullOrEmpty(id))
                        {
                            var sendOperationResult = new SendOperationResult
                            {
                                SendResult = mappedResult,
                                GatewayReference = reference ?? string.Empty
                            };

                            if (!string.IsNullOrEmpty(reference))
                            {
                                activity.SetTag("sendOperationResult", JsonSerializer.Serialize(sendOperationResult));
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // Silently ignore parsing errors - the controller will handle validation
                }
            }
        }

        // Continue to the next middleware in the pipeline
        await _next(context);
    }
}
