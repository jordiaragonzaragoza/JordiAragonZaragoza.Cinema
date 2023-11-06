﻿namespace JordiAragon.Cinemas.Reservation.Auditorium.Presentation.WebApi.V2
{
    using System.Collections.Generic;
    using Ardalis.Result;
    using AutoMapper;
    using JordiAragon.Cinemas.Reservation.Auditorium.Application.Contracts.Queries;
    using JordiAragon.Cinemas.Reservation.Presentation.WebApi.Contracts.V2.Auditorium.Responses;
    using JordiAragon.Cinemas.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinemas.Reservation.Showtime.Application.Contracts.Commands;

    public class AuditoriumsMapper : Profile
    {
        public AuditoriumsMapper()
        {
            // Requests to queries or commands.
            this.CreateMap<CreateShowtimeRequest, CreateShowtimeCommand>();

            // OutputDtos to responses.
            this.CreateMap<SeatOutputDto, SeatResponse>();
            this.CreateMap<Result<IEnumerable<SeatOutputDto>>, Result<IEnumerable<SeatResponse>>>();
            this.CreateMap<AuditoriumOutputDto, AuditoriumResponse>();
            this.CreateMap<Result<IEnumerable<AuditoriumOutputDto>>, Result<IEnumerable<AuditoriumResponse>>>();
        }
    }
}