namespace JordiAragon.Cinema.Application.Features.Auditorium.Events
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Application.Contracts.Features.Showtime.Events;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate.Specifications;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using MediatR;
    using NotFoundException = JordiAragon.SharedKernel.Domain.Exceptions.NotFoundException;

    public class ShowtimeDeletedEventHandler : INotificationHandler<ShowtimeDeletedEvent>
    {
        private readonly IRepository<Auditorium> auditoriumRepository;

        public ShowtimeDeletedEventHandler(
            IRepository<Auditorium> auditoriumRepository)
        {
            this.auditoriumRepository = Guard.Against.Null(auditoriumRepository, nameof(auditoriumRepository));
        }

        public async Task Handle(ShowtimeDeletedEvent @event, CancellationToken cancellationToken)
        {
            var existingAuditorium = await this.auditoriumRepository.FirstOrDefaultAsync(new AuditoriumByIdSpec(AuditoriumId.Create(@event.AuditoriumId)), cancellationToken)
                                    ?? throw new NotFoundException(nameof(Auditorium), @event.AuditoriumId.ToString());

            existingAuditorium.RemoveShowtime(ShowtimeId.Create(@event.ShowtimeId));

            await this.auditoriumRepository.UpdateAsync(existingAuditorium, cancellationToken);
        }
    }
}