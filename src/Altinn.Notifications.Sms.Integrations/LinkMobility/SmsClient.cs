using Altinn.Notifications.Sms.Core;
using Altinn.Notifications.Sms.Core.Integrations;
using Altinn.Notifications.Sms.Core.Integrations.Interfaces;
using Altinn.Notifications.Sms.Core.Status;

using LinkMobility.PSWin.Client;
using LinkMobility.PSWin.Client.Model;
using LinkMobility.PSWin.Client.Transports;

using Microsoft.Extensions.Options;

using LinkMobilityModel = global::LinkMobility.PSWin.Client.Model;

namespace Altinn.Notifications.Sms.Integrations.LinkMobility
{
    public class SmsClient : ISmsClient
    {
        private readonly GatewayClient _client;

        public SmsClient(IOptions<SmsGatewayConfiguration> gatewaySettings)
        {
            SmsGatewayConfiguration settings = gatewaySettings.Value;
            _client = new(new XmlTransport(settings.Username, settings.Password, new Uri(settings.Endpoint)));
        }

        /// <inheritdoc />
        public async Task<Result<string, SmsClientErrorResponse>> SendSmsAsync(Core.Sending.Sms sms)
        {
            MessageResult result = await _client.SendAsync(new LinkMobilityModel.Sms(sms.Recipient, sms.Message, sms.Sender));

            if (result.IsStatusOk)
            {
                return result.GatewayReference;
            }

            if (result.StatusText.StartsWith("Invalid RCV"))
            {
                return new SmsClientErrorResponse { SendResult = SmsSendResult.Failed_InvalidReceiver, ErrorMessage = result.StatusText };

            }

            return new SmsClientErrorResponse { SendResult = SmsSendResult.Failed };
        }
    }
}
