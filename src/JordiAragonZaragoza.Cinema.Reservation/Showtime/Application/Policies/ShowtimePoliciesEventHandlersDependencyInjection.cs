namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Policies
{
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Policies.ShowtimeCanceled;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Policies.ShowtimeEnded;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Policies.ShowtimeScheduled;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using Microsoft.Extensions.DependencyInjection;

    public static class ShowtimePoliciesEventHandlersDependencyInjection
    {
        public static IServiceCollection AddShowtimeEventHandlersPolicies(this IServiceCollection services)
        {
            // ShowtimeScheduled policies.
            services.AddPolicyEventHandler<ShowtimeScheduledEvent, ScheduleShowtimeInAuditoriumPolicy>();
            services.AddPolicyEventHandler<ShowtimeScheduledEvent, ScheduleShowtimeInMoviePolicy>();

            // ShowtimeCanceled policies.
            services.AddPolicyEventHandler<ShowtimeCanceledEvent, CancelShowtimeInAuditoriumPolicy>();
            services.AddPolicyEventHandler<ShowtimeCanceledEvent, CancelShowtimeInMoviePolicy>();

            // ShowtimeEnded policies.
            services.AddPolicyEventHandler<ShowtimeEndedEvent, EndShowtimeInAuditoriumPolicy>();
            services.AddPolicyEventHandler<ShowtimeEndedEvent, EndShowtimeInMoviePolicy>();

            return services;
        }
    }
}