namespace JordiAragon.Cinema.Reservation.Showtime.Application.Projections
{
    using System;
    using AutoMapper;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;

    public class ShowtimeMapper : Profile
    {
        public ShowtimeMapper()
        {
            this.CreateMap<ShowtimeId, Guid>()
                .ConvertUsing(src => src.Value);

            this.CreateMap<TicketId, Guid>()
                .ConvertUsing(src => src.Value);
        }
    }
}