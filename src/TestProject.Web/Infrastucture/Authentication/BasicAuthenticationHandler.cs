using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;


namespace TestProject.Web.Infrastucture.Authentication;

// BasicAuthenticationHandler.cs
public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
  public BasicAuthenticationHandler(
      IOptionsMonitor<AuthenticationSchemeOptions> options,
      ILoggerFactory logger,
      UrlEncoder encoder,
      ISystemClock clock,
      IOptions<BasicAuthenticationInfo> _options
      )
: base(options, logger, encoder, clock)
  {
    _authenticationInfo = _options.Value;
  }

  private readonly BasicAuthenticationInfo _authenticationInfo;

  protected override Task<AuthenticateResult> HandleAuthenticateAsync()
  {
    Response.Headers.Add("WWW-Authenticate", "Basic");

    if (!Request.Headers.ContainsKey(HeaderNames.Authorization))
    {
      return Task.FromResult(AuthenticateResult.Fail("Authorization header missing."));
    }

    // Get authorization key
    var authorizationHeader = Request.Headers[HeaderNames.Authorization].ToString();
    var authHeaderRegex = new Regex(@"Basic (.*)");

    if (!authHeaderRegex.IsMatch(authorizationHeader))
    {
      return Task.FromResult(AuthenticateResult.Fail("Authorization code not formatted properly."));
    }

    var authBase64 = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderRegex.Replace(authorizationHeader, "$1")));
    var authSplit = authBase64.Split(Convert.ToChar(":"), 2);
    var authUsername = authSplit[0];
    var authPassword = authSplit.Length > 1 ? authSplit[1] : throw new Exception("Unable to get password");

    if (authUsername != _authenticationInfo.Username || authPassword != _authenticationInfo.Password)
    {
      return Task.FromResult(AuthenticateResult.Fail("The username or password is not correct."));
    }

    var authenticatedUser = new AuthenticatedUser("BasicAuthentication", true, _authenticationInfo.Username);
    var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(authenticatedUser));

    return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
  }
}