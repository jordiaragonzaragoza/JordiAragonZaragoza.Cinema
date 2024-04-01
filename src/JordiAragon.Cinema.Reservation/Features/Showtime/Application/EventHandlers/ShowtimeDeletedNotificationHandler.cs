﻿namespace JordiAragon.Cinema.Reservation.Showtime.Application.EventHandlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Events;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public sealed class ShowtimeDeletedNotificationHandler : INotificationHandler<ShowtimeDeletedNotification>
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

            // TODO: Use event bus to notify showtime creation integration event out side from source transaction. Use some service to notify the admin.
            return Task.CompletedTask;
        }
    }
}