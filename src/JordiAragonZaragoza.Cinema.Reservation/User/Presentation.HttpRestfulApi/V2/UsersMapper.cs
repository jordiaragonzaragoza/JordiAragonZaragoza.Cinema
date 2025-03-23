namespace JordiAragonZaragoza.Cinema.Reservation.User.Presentation.HttpRestfulApi.V2
{
    using System.Collections.Generic;
    using Ardalis.Result;
    using AutoMapper;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.User.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.User.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;

    public sealed class UsersMapper : Profile
    {
        public UsersMapper()
        {
            // Requests to queries or commands.
            this.CreateMap<UserReservationRequest, GetUserReservationQuery>();
            this.CreateMap<UserReservationsRequest, GetUserReservationsQuery>();

            // OutputDtos to responses.
            this.CreateMap<UserOutputDto, UserResponse>();
            this.CreateMap<Result<IEnumerable<UserOutputDto>>, Result<IEnumerable<UserResponse>>>();
        }
    }
}