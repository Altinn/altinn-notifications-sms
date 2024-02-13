﻿using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Altinn.Notifications.Sms.Configuration;

/// <summary>
/// Some basic auth handler
/// </summary>
public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly UserSettings _userSettings;

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="options">options</param>
    /// <param name="logger">logger</param>
    /// <param name="encoder">encoder</param>
    /// <param name="clock">clock</param>
    /// <param name="userSettings">userSettings</param>
    [Obsolete]
    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        UserSettings userSettings)
        : base(options, logger, encoder, clock)
    {
        _userSettings = userSettings;
    }

    /// <summary>
    /// Authenticate
    /// </summary>
    /// <returns></returns>
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var username = string.Empty;
        var password = string.Empty;

        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return AuthenticateResult.Fail("Missing Authorization Header");
        }

        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
            username = credentials[0];
            password = credentials[1];
        }
        catch
        {
            return AuthenticateResult.Fail("Invalid Authorization Header");
        }

        if (username != _userSettings.Username || password != _userSettings.Password)
        {
            return AuthenticateResult.Fail("Invalid Username or Password");
        }

        var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
            };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}