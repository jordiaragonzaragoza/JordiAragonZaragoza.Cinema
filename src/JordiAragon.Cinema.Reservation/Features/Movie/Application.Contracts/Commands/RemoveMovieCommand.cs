namespace JordiAragon.Cinema.Reservation.Features.Movie.Application.Contracts.Commands
{
    using System;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class RemoveMovieCommand(Guid MovieId) : ICommand;
}