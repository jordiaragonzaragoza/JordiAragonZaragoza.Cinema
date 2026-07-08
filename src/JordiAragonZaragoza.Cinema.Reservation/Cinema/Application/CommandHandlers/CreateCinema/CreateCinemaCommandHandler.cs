namespace JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.CommandHandlers.CreateCinema
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Domain;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class CreateCinemaCommandHandler : ICommandHandler<CreateCinemaCommand>
    {
        private readonly IRepository<Cinema, CinemaId> cinemaRepository;

        public CreateCinemaCommandHandler(IRepository<Cinema, CinemaId> cinemaRepository)
        {
            this.cinemaRepository = cinemaRepository ?? throw new ArgumentNullException(nameof(cinemaRepository));
        }

        public async Task<Result> Handle(CreateCinemaCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var newCinema = Cinema.Create(
                id: new CinemaId(request.CinemaId));

            await this.cinemaRepository.AddAsync(newCinema, cancellationToken);

            return Result.NoContent();
        }
    }
}