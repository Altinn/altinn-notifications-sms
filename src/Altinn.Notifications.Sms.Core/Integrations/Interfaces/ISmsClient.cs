namespace Altinn.Notifications.Sms.Core.Integrations.Interfaces
{
    /// <summary>
    /// This interface describes the public interface of a client able to send sms messages through an sms service.
    /// </summary>  
    public interface ISmsClient
    {
        /// <summary>
        /// Method for requesting the sending on an sms message.
        /// </summary>
        /// <param name="sms">The sms to be sent</param>
        /// <returns>An id for tracing the sucess of the task or an <see cref="SmsClientErrorResponse"/> it the task fails</returns>
        public Task<Result<string, SmsClientErrorResponse>> SendSmsAsync(Sending.Sms sms);
    }
}
