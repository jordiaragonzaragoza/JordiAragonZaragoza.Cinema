namespace JordiAragonZaragoza.Cinema.Reservation.FunctionalTests
{
    using System;
    using System.Net.Http.Headers;
    using System.Security.Claims;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Identity;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.Net.Http.Headers;

    /// <summary>
    /// Replaces JWT validation in functional tests. Authenticates only requests that carry
    /// "Authorization: Test {userId}"; any other request is left unauthenticated (so 401 can be tested).
    /// </summary>
    public sealed class AuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string SchemeName = "Test";

        private readonly string userIdClaimType;

        public AuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            IOptions<IdentityClaimsOptions> identityClaimsOptions)
            : base(options, logger, encoder)
        {
            ArgumentNullException.ThrowIfNull(identityClaimsOptions);
            this.userIdClaimType = identityClaimsOptions.Value.UserIdClaimType;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!this.Request.Headers.TryGetValue(HeaderNames.Authorization, out var header)
                || !AuthenticationHeaderValue.TryParse(header, out var value)
                || !string.Equals(value.Scheme, SchemeName, StringComparison.Ordinal))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var userId = Guid.TryParse(value.Parameter, out var parsed) ? parsed : SeedData.ExampleUserWithAdminRole.Id;

            // Claim names must match what the API reads (MapInboundClaims = false => raw JWT names).
            var identity = new ClaimsIdentity(
                [
                    new Claim(this.userIdClaimType, userId.ToString()),
                ],
                SchemeName);

            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), SchemeName);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
