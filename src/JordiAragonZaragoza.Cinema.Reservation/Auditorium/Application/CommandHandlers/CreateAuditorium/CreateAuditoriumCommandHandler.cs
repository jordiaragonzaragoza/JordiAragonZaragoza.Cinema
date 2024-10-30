namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.CommandHandlers.CreateAuditorium
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.SharedKernel.Application.Commands;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class CreateAuditoriumCommandHandler : BaseCommandHandler<CreateAuditoriumCommand>
    {
        private readonly IRepository<Auditorium, AuditoriumId> auditoriumRepository;

        public CreateAuditoriumCommandHandler(IRepository<Auditorium, AuditoriumId> auditoriumRepository)
        {
            this.auditoriumRepository = Guard.Against.Null(auditoriumRepository, nameof(auditoriumRepository));
        }

        public override async Task<Result> Handle(CreateAuditoriumCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));

            // TODO: There cannot be two Auditoriums with the same name and same cinema id.
            // This rule is part from cinema manager bounded context using a domain service.
            var newAuditorium = Auditorium.Create(
                id: AuditoriumId.Create(request.AuditoriumId),
                name: request.Name,
                rows: Rows.Create(request.Rows),
                seatsPerRow: SeatsPerRow.Create(request.SeatsPerRow));

            await this.auditoriumRepository.AddAsync(newAuditorium, cancellationToken);

            return Result.Success();
        }
    }
}