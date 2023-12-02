namespace JordiAragon.Cinema.Reservation.Showtime.Application.Events
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Reservation.Showtime.Domain.Notifications;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class ShowtimeCreatedNotificationHandler : INotificationHandler<ShowtimeCreatedNotification>
    {
        private readonly ILogger<ShowtimeCreatedNotificationHandler> logger;

        public ShowtimeCreatedNotificationHandler(
            ILogger<ShowtimeCreatedNotificationHandler> logger)
        {
            this.logger = Guard.Against.Null(logger, nameof(logger));
        }

        public Task Handle(ShowtimeCreatedNotification notification, CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Handled Notification: {Event}", notification.GetType().Name);

            // TODO: Use some service to notify the admin.
            return Task.CompletedTask;
        }
    }
}