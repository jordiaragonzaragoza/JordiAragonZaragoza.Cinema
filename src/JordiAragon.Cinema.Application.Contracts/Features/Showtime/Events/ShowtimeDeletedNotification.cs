namespace JordiAragon.Cinema.Application.Contracts.Features.Showtime.Events
{
    using JordiAragon.SharedKernel.Application.Contracts.Events;

    public record class ShowtimeDeletedNotification : BaseApplicationEventNotification<ShowtimeDeletedEvent>
    {
        public ShowtimeDeletedNotification(ShowtimeDeletedEvent @event)
            : base(@event)
        {
        }
    }
}