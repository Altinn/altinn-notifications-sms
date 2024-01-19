using Altinn.Notifications.Sms.Core.Status;

namespace Altinn.Notifications.Sms.Core.Integrations
{
    public class SmsClientErrorResponse
    {
        public SmsSendResult SendResult { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
