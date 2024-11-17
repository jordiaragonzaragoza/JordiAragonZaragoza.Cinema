namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels
{
    using System;

    // TODO: This output dto will be a read model on moving to management bounded context.
    public sealed record class AuditoriumOutputDto(Guid Id, string Name);
}