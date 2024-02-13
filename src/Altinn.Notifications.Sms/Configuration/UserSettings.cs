namespace Altinn.Notifications.Sms.Configuration;

/// <summary>
/// Configuration object used to hold user settings to access endpoint.
/// </summary>
public class UserSettings
{
    /// <summary>
    /// The username
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// The password
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
