namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.CommandHandlers.CreateAuditorium
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Domain;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class CreateAuditoriumCommandHandler : ICommandHandler<CreateAuditoriumCommand>
    {
        private readonly IRepository<Auditorium, AuditoriumId> auditoriumRepository;

        public CreateAuditoriumCommandHandler(IRepository<Auditorium, AuditoriumId> auditoriumRepository)
        {
            this.auditoriumRepository = auditoriumRepository ?? throw new ArgumentNullException(nameof(auditoriumRepository));
        }

        public async Task<Result> Handle(CreateAuditoriumCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            // TODO: There cannot be two Auditoriums with the same name and same cinema id.
            // This rule is part from cinema manager bounded context using a domain service.
            var newAuditorium = Auditorium.Create(
                id: new AuditoriumId(request.AuditoriumId),
                cinemaId: new CinemaId(request.CinemaId),
                name: Name.Create(request.Name),
                rows: Rows.Create(request.Rows),
                seatsPerRow: SeatsPerRow.Create(request.SeatsPerRow));

            await this.auditoriumRepository.AddAsync(newAuditorium, cancellationToken);

            return Result.NoContent();
        }
    }
}