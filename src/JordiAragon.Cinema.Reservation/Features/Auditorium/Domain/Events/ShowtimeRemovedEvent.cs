﻿namespace JordiAragon.Cinema.Reservation.Auditorium.Domain.Events
{
    using System;
    using JordiAragon.SharedKernel.Domain.Events;

    public sealed record class ShowtimeRemovedEvent(Guid AggregateId, Guid ShowtimeId) : BaseDomainEvent(AggregateId);
}