namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Infrastructure.Api.Command
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime;

    public sealed class CommandService
    {
        private readonly HttpClient httpClient;

        public CommandService(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<string> CancelShowtimeAsync(Guid showtimeId, CancellationToken cancellationToken = default)
        {
            var route = $"{Routes.ApiBase}{ShowtimeRoutes.CancelShowtime}";
            route = route.Replace("{showtimeId}", showtimeId.ToString(), StringComparison.Ordinal);

            var fullUri = new Uri(this.httpClient.BaseAddress!, route);

            using var response = await this.httpClient.DeleteAsync(fullUri, cancellationToken);

            response.EnsureSuccessStatusCode();

            return $"Showtime {showtimeId} cancelled successfully.";
        }
    }
}