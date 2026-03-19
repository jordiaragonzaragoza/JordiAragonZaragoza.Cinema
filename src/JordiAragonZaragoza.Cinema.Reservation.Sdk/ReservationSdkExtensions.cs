namespace JordiAragonZaragoza.Cinema.Reservation.Sdk
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Sdk.Command.V2;
    using JordiAragonZaragoza.Cinema.Reservation.Sdk.Query.V2;
    using Microsoft.Extensions.DependencyInjection;

    public static class ReservationSdkExtensions
    {
        public static IHttpClientBuilder AddReservationCommandClient(
            this IServiceCollection services,
            Uri baseAddress)
        {
            return services.AddHttpClient<IReservationCommandClient, ReservationCommandClient>(
                client => client.BaseAddress = baseAddress);
        }

        public static IHttpClientBuilder AddReservationCommandClient(
            this IServiceCollection services,
            string baseAddress)
        {
            return services.AddReservationCommandClient(new Uri(baseAddress));
        }

        public static IHttpClientBuilder AddReservationQueryClient(
            this IServiceCollection services,
            Uri baseAddress)
        {
            return services.AddHttpClient<IReservationQueryClient, ReservationQueryClient>(
                client => client.BaseAddress = baseAddress);
        }

        public static IHttpClientBuilder AddReservationQueryClient(
            this IServiceCollection services,
            string baseAddress)
        {
            return services.AddReservationQueryClient(new Uri(baseAddress));
        }
    }
}