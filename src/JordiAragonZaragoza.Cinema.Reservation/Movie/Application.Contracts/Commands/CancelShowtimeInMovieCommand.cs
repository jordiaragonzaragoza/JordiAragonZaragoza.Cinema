namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.Commands
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class CancelShowtimeInMovieCommand(
        Guid MovieId,
        Guid ShowtimeId) : ICommand;
}