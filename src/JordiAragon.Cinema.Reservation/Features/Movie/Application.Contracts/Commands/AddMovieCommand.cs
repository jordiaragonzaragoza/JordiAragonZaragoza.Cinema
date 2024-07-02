namespace JordiAragon.Cinema.Reservation.Features.Movie.Application.Contracts.Commands
{
    using System;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class AddMovieCommand(
        Guid MovieId,
        string Title,
        TimeSpan Runtime,
        DateTimeOffset StartingPeriod,
        DateTimeOffset EndOfPeriod)
        : ICommand;
}