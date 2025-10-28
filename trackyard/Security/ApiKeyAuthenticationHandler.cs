using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Sprint1CSharp.Security
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string HeaderName = "X-API-KEY";
        private readonly IConfiguration _configuration;

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IConfiguration configuration) : base(options, logger, encoder, clock)
        {
            _configuration = configuration;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Lê a api key configurada
            var configuredKey = _configuration["ApiKey"] ?? Environment.GetEnvironmentVariable("API_KEY");
            if (string.IsNullOrWhiteSpace(configuredKey))
            {
                return Task.FromResult(AuthenticateResult.Fail("API Key não configurada"));
            }

            if (!Request.Headers.TryGetValue(HeaderName, out var provided))
            {
                return Task.FromResult(AuthenticateResult.Fail("Está faltando a  API Key"));
            }

            if (!string.Equals(configuredKey, provided.ToString(), StringComparison.Ordinal))
            {
                return Task.FromResult(AuthenticateResult.Fail("API Key inválida"));
            }

            var claims = new[] { new Claim(ClaimTypes.Name, "ApiKeyUser") };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}

