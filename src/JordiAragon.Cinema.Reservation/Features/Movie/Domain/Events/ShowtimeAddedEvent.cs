﻿namespace JordiAragon.Cinema.Reservation.Movie.Domain.Events
{
    using System;
    using JordiAragon.SharedKernel.Domain.Events;

    public sealed record class ShowtimeAddedEvent(Guid AggregateId, Guid ShowtimeId) : BaseDomainEvent(AggregateId);
}