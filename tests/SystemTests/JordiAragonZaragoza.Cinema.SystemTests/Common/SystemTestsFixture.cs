namespace JordiAragonZaragoza.Cinema.SystemTests.Common
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Aspire.Hosting;
    using Aspire.Hosting.Testing;
    using JordiAragonZaragoza.Cinema.Reservation.Sdk.Command.V2;
    using JordiAragonZaragoza.Cinema.Reservation.Sdk.Query.V2;
    using JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder;
    using JordiAragonZaragoza.Cinema.SharedKernel;
    using JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime;
    using Xunit;

    public class SystemTestsFixture : IAsyncLifetime
    {
        private static readonly TimeSpan StartupTimeout = TimeSpan.FromMinutes(3);

        private DistributedApplication app = null!;
        private HttpClient commandHttp = null!;
        private HttpClient queryHttp = null!;

        public ReservationCommandTestClient ReservationCommandTestClient { get; private set; } = default!;

        public ReservationQueryTestClient ReservationQueryTestClient { get; private set; } = default!;

        public async Task InitializeAsync()
        {
            var appHost = await DistributedApplicationTestingBuilder
                                .CreateAsync<Projects.JordiAragonZaragoza_Cinema>(
                                [
                                    "DcpPublisher:RandomizePorts=false",
                                    "--environment=Development",
                                    "IsTesting=true",
                                ],
                                configureBuilder: (appOptions, hostSettings) =>
                                {
                                    appOptions.DisableDashboard = true;
                                });

            this.app = await appHost.BuildAsync();
            await this.app.StartAsync();

            // The realm import happens while Keycloak boots: wait before asking for a token.
            await this.app.ResourceNotifications
                .WaitForResourceHealthyAsync(Constants.Keycloak)
                .WaitAsync(StartupTimeout);

            var accessToken = await this.GetAccessTokenAsync();

            this.commandHttp = this.app.CreateHttpClient(Constants.ReservationApiCommand);
            ConfigureAuthenticatedClient(this.commandHttp, accessToken);
            var reservationCommandClient = new ReservationCommandClient(this.commandHttp);
            this.ReservationCommandTestClient = new ReservationCommandTestClient(reservationCommandClient);

            this.queryHttp = this.app.CreateHttpClient(Constants.ReservationApiQuery);
            ConfigureAuthenticatedClient(this.queryHttp, accessToken);
            var reservationQueryClient = new ReservationQueryClient(this.queryHttp);
            this.ReservationQueryTestClient = new ReservationQueryTestClient(reservationQueryClient);
        }

        public async Task DisposeAsync()
        {
            this.commandHttp.Dispose();
            this.queryHttp.Dispose();

            await this.app.DisposeAsync();
        }

        private static void ConfigureAuthenticatedClient(HttpClient client, string accessToken)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // The user is now a real actor: the middleware requires the scope headers and
            // validates that the user has access to them (these ones are seeded for the admin user).
            client.DefaultRequestHeaders.Add("x-tenant-id", SeedData.ExampleTenant.Id.ToString());
            client.DefaultRequestHeaders.Add("x-partition-id", SeedData.ExamplePartition.Id.ToString());
            client.DefaultRequestHeaders.Add("x-domain-id", SeedData.ExampleCinema.Id.ToString());
        }

        private async Task<string> GetAccessTokenAsync()
        {
            // Same host the APIs use as Authority (localhost:8080, fixed in the AppHost).
            var keycloakAddress = this.app.GetEndpoint(Constants.Keycloak, "http");

            using var tokenClient = new KeycloakTokenClient(keycloakAddress);
            return await tokenClient.GetAccessTokenAsync(SystemTestUser.UserName, SystemTestUser.Password);
        }
    }
}