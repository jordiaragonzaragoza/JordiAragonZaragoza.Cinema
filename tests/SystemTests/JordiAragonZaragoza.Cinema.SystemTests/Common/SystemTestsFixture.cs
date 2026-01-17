namespace JordiAragonZaragoza.Cinema.SystemTests.Common
{
    using System.Threading.Tasks;
    using Aspire.Hosting;
    using Aspire.Hosting.Testing;
    using JordiAragonZaragoza.Cinema.SharedKernel;
    using JordiAragonZaragoza.Cinema.SystemTests.Common.ApiClients;
    using Xunit;

    public class SystemTestsFixture : IAsyncLifetime
    {
        private DistributedApplication app = null!;

        public ReservationCommandClient ReservationCommandClient { get; private set; } = default!;

        public ReservationQueryClient ReservationQueryClient { get; private set; } = default!;

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

            this.ReservationCommandClient = new ReservationCommandClient(this.app.CreateHttpClient(Constants.ReservationApiCommand));
            this.ReservationQueryClient = new ReservationQueryClient(this.app.CreateHttpClient(Constants.ReservationApiQuery));
        }

        public async Task DisposeAsync()
            => await this.app.DisposeAsync();
    }
}