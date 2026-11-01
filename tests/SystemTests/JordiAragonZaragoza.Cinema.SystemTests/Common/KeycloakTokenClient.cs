namespace JordiAragonZaragoza.Cinema.SystemTests.Common
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.SharedKernel;

    /// <summary>
    /// Gets access tokens from the Keycloak container started by the AppHost.
    /// Uses the password grant of a development-only client, which is the
    /// non-interactive equivalent of a user logging in: the API validates a real JWT.
    /// </summary>
    public sealed class KeycloakTokenClient : IDisposable
    {
        public const string ClientId = "cinema-system-tests";

        private readonly HttpClient http = new();
        private readonly Uri tokenEndpoint;

        public KeycloakTokenClient(Uri keycloakBaseAddress)
        {
            ArgumentNullException.ThrowIfNull(keycloakBaseAddress);

            // The base address host must be the same one the APIs use as Authority
            // (localhost:8080), otherwise the 'iss' claim will not match and the API returns 401.
            this.tokenEndpoint = new Uri(
                keycloakBaseAddress,
                $"realms/{Constants.KeycloakRealm}/protocol/openid-connect/token");
        }

        public async Task<string> GetAccessTokenAsync(
            string userName,
            string password,
            TimeSpan? timeout = null,
            CancellationToken cancellationToken = default)
        {
            var limit = DateTime.UtcNow + (timeout ?? TimeSpan.FromSeconds(90));
            var lastError = "no attempt made";

            while (DateTime.UtcNow < limit)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    using var content = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        ["grant_type"] = "password",
                        ["client_id"] = ClientId,
                        ["username"] = userName,
                        ["password"] = password,
                        ["scope"] = "openid",
                    });

                    using var response = await this.http.PostAsync(this.tokenEndpoint, content, cancellationToken);
                    var body = await response.Content.ReadAsStringAsync(cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        using var json = JsonDocument.Parse(body);
                        return json.RootElement.GetProperty("access_token").GetString()!;
                    }

                    var status = (int)response.StatusCode;
                    lastError = $"{status}: {body}";

                    // 404 (realm not imported yet) and 5xx are transient while Keycloak boots.
                    // Anything else (400 invalid_grant, 401 invalid client...) is a configuration error.
                    if (status != 404 && status < 500)
                    {
                        throw new InvalidOperationException($"Keycloak rejected the token request. {lastError}");
                    }
                }
                catch (HttpRequestException exception)
                {
                    lastError = exception.Message;
                }

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }

            throw new TimeoutException($"Could not get a token from Keycloak. Last error: {lastError}");
        }

        public void Dispose() => this.http.Dispose();
    }
}
