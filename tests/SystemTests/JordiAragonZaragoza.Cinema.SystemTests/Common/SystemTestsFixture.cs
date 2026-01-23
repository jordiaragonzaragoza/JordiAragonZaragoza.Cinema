namespace JordiAragonZaragoza.Cinema.SystemTests.Common
{
    using System.Threading.Tasks;
    using Aspire.Hosting;
    using Aspire.Hosting.Testing;
    using JordiAragonZaragoza.Cinema.SharedKernel;
    using JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime;

    using Xunit;

    public class SystemTestsFixture : IAsyncLifetime
    {
        private DistributedApplication app = null!;

        public ShowtimeCommandClient ShowtimeCommandClient { get; private set; } = default!;

        public ShowtimeQueryClient ShowtimeQueryClient { get; private set; } = default!;

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

            var commandHttp = this.app.CreateHttpClient(Constants.ReservationApiCommand);
            var queryHttp = this.app.CreateHttpClient(Constants.ReservationApiQuery);

            this.ShowtimeCommandClient = new ShowtimeCommandClient(commandHttp);
            this.ShowtimeQueryClient = new ShowtimeQueryClient(queryHttp);
        }

        public async Task DisposeAsync()
            => await this.app.DisposeAsync();
    }
}