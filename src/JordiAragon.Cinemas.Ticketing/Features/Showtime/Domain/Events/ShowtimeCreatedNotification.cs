﻿namespace JordiAragon.Cinemas.Ticketing.Showtime.Domain.Events
{
    using JordiAragon.SharedKernel.Domain.Events;

    public record class ShowtimeCreatedNotification(ShowtimeCreatedEvent Event)
        : BaseDomainEventNotification<ShowtimeCreatedEvent>(Event);
}