﻿namespace JordiAragon.Cinema.Domain.AuditoriumAggregate.Events
{
    using System;
    using JordiAragon.SharedKernel.Domain.Events;

    public record class ShowtimeAddedEvent(Guid ShowtimeId) : BaseDomainEvent;
}