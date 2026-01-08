namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Policies
{
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Events;

    public static class ShowtimePoliciesEventHandlersDependencyInjection
    {
        public static IServiceCollection AddShowtimePolicies(this IServiceCollection services)
        {
            // ShowtimeScheduled policies.
            services.AddPolicyEventHandler<ShowtimeScheduledEvent, ShowtimeScheduled.AuditoriumAddActiveShowtimePolicy>();
            services.AddPolicyEventHandler<ShowtimeScheduledEvent, ShowtimeScheduled.MovieAddActiveShowtimePolicy>();

            // ShowtimeCanceled policies.
            services.AddPolicyEventHandler<ShowtimeCanceledEvent, ShowtimeCanceled.AuditoriumRemoveActiveShowtimePolicy>();
            services.AddPolicyEventHandler<ShowtimeCanceledEvent, ShowtimeCanceled.MovieRemoveActiveShowtimePolicy>();

            // ShowtimeEnded policies.
            services.AddPolicyEventHandler<ShowtimeEndedEvent, ShowtimeEnded.AuditoriumRemoveActiveShowtimePolicy>();
            services.AddPolicyEventHandler<ShowtimeEndedEvent, ShowtimeEnded.MovieRemoveActiveShowtimePolicy>();

            return services;
        }
    }
}