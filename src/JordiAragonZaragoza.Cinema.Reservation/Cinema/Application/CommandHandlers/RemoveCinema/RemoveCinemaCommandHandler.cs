namespace JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.CommandHandlers.RemoveCinema
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Domain;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class RemoveCinemaCommandHandler : ICommandHandler<RemoveCinemaCommand>
    {
        private readonly IRepository<Cinema, CinemaId> cinemaRepository;

        public RemoveCinemaCommandHandler(IRepository<Cinema, CinemaId> cinemaRepository)
        {
            this.cinemaRepository = cinemaRepository ?? throw new ArgumentNullException(nameof(cinemaRepository));
        }

        public async Task<Result> Handle(RemoveCinemaCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var existingCinema = await this.cinemaRepository.GetByIdAsync(new CinemaId(request.CinemaId), cancellationToken);
            if (existingCinema is null)
            {
                return Result.NotFound($"{nameof(Cinema)}: {request.CinemaId} not found.");
            }

            // TODO: Before remove Cinema check if there is some scheduled showtime regarding to Cinema via domain service.
            existingCinema.Remove();

            await this.cinemaRepository.DeleteAsync(existingCinema, cancellationToken);

            return Result.NoContent();
        }
    }
}