namespace JordiAragonZaragoza.Cinema.SystemTests.Common
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Aspire.Hosting;
    using Aspire.Hosting.Testing;
    using Xunit;
    using JordiAragonZaragoza.Cinema.SharedKernel;

    public class SystemTestsFixture : IAsyncLifetime
    {
        private DistributedApplication app = null!;

        public HttpClient ReservationApiCommandHttpClient { get; private set; } = default!;

        public HttpClient ReservationApiQueryHttpClient { get; private set; } = default!;

        public async Task InitializeAsync()
        {
            var appHost = await DistributedApplicationTestingBuilder
                                .CreateAsync<Projects.JordiAragonZaragoza_Cinema>(
                                [
                                    "DcpPublisher:RandomizePorts=false",
                                    "--environment=Development",
                                    "UseVolumes=false",
                                ]);

            this.app = await appHost.BuildAsync();

            await this.app.StartAsync();

            this.ReservationApiCommandHttpClient = this.app.CreateHttpClient(Constants.ReservationApiCommand);
            this.ReservationApiQueryHttpClient = this.app.CreateHttpClient(Constants.ReservationApiQuery);
        }

        public async Task DisposeAsync()
            => await this.app.DisposeAsync();
    }
}