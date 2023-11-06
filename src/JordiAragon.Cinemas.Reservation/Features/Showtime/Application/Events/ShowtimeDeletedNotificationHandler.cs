namespace JordiAragon.Cinemas.Reservation.Showtime.Application.Events
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinemas.Reservation.Showtime.Application.Contracts.Events;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class ShowtimeDeletedNotificationHandler : INotificationHandler<ShowtimeDeletedNotification>
    {
        private readonly ILogger<ShowtimeDeletedNotificationHandler> logger;

        public ShowtimeDeletedNotificationHandler(
            ILogger<ShowtimeDeletedNotificationHandler> logger)
        {
            this.logger = Guard.Against.Null(logger, nameof(logger));
        }

        public Task Handle(ShowtimeDeletedNotification notification, CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Handled Notification: {Event}", notification.GetType().Name);

            // TODO: Use some service to notify the admin.
            return Task.CompletedTask;
        }
    }
}