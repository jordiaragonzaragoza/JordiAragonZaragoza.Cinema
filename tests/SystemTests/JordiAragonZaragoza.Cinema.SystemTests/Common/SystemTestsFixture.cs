namespace JordiAragonZaragoza.Cinema.SystemTests.Common
{
    using System.Threading.Tasks;
    using System.Net.Http;
    using Aspire.Hosting;
    using Aspire.Hosting.Testing;
    using JordiAragonZaragoza.Cinema.Reservation.Sdk.Command.V2;
    using JordiAragonZaragoza.Cinema.Reservation.Sdk.Query.V2;
    using JordiAragonZaragoza.Cinema.SharedKernel;
    using JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime;
    using Xunit;

    public class SystemTestsFixture : IAsyncLifetime
    {
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
                                    "IsTesting=true"
                                ]);

            this.app = await appHost.BuildAsync();
            await this.app.StartAsync();

            this.commandHttp = this.app.CreateHttpClient(Constants.ReservationApiCommand);
            var reservationCommandClient = new ReservationCommandClient(this.commandHttp);
            this.ReservationCommandTestClient = new ReservationCommandTestClient(reservationCommandClient);

            this.queryHttp = this.app.CreateHttpClient(Constants.ReservationApiQuery);
            var reservationQueryClient = new ReservationQueryClient(this.queryHttp);
            this.ReservationQueryTestClient = new ReservationQueryTestClient(reservationQueryClient);
        }

        public async Task DisposeAsync()
        {
            this.commandHttp.Dispose();
            this.queryHttp.Dispose();

            await this.app.DisposeAsync();
        }
    }
}