namespace JordiAragonZaragoza.Cinema.SystemTests.Common
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Aspire.Hosting;
    using Aspire.Hosting.Testing;
    using Xunit;
    using JordiAragonZaragoza.Cinema.SharedKernel;
    using Aspire.Hosting.ApplicationModel;
    using Microsoft.Extensions.DependencyInjection;

    public class SystemTestsFixture : IAsyncLifetime
    {
        private DistributedApplication app = null!;

        public HttpClient ReservationHttpClient { get; private set; } = default!;

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

            var resourceNotificationService = this.app.Services.GetRequiredService<ResourceNotificationService>();

            await this.app.StartAsync();

            this.ReservationHttpClient = this.app.CreateHttpClient(Constants.JordiAragonZaragozaCinemaReservation);

            await WaitForResources(resourceNotificationService);
        }

        public async Task DisposeAsync()
            => await this.app.DisposeAsync();

        private static async Task WaitForResources(ResourceNotificationService resourceNotificationService)
        {
            var postgresServerTask = resourceNotificationService.WaitForResourceAsync(
                            Constants.PostgresServer,
                            KnownResourceStates.Running)
                            .WaitAsync(TimeSpan.FromSeconds(10));

            var eventStoreDbServerTask = resourceNotificationService.WaitForResourceAsync(
                            Constants.EventStoreDbServer,
                            KnownResourceStates.Running)
                            .WaitAsync(TimeSpan.FromSeconds(10));

            var seqServerTask = resourceNotificationService.WaitForResourceAsync(
                            Constants.SeqServer,
                            KnownResourceStates.Running)
                            .WaitAsync(TimeSpan.FromSeconds(10));

            var reservationTask = resourceNotificationService.WaitForResourceAsync(
                            Constants.JordiAragonZaragozaCinemaReservation,
                            KnownResourceStates.Running)
                            .WaitAsync(TimeSpan.FromSeconds(10));

            await Task.WhenAll(
                postgresServerTask,
                eventStoreDbServerTask,
                seqServerTask,
                reservationTask);

            await Task.Delay(TimeSpan.FromSeconds(8));
        }
    }
}