namespace JordiAragon.Cinema.Reservation.User.Presentation.WebApi.V2
{
    using System.Collections.Generic;
    using Ardalis.Result;
    using AutoMapper;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.User.Requests;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.User.Responses;
    using JordiAragon.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragon.Cinema.Reservation.User.Application.Contracts.ReadModels;

    public class UsersMapper : Profile
    {
        public UsersMapper()
        {
            // Requests to queries or commands.
            this.CreateMap<UserTicketRequest, GetUserTicketQuery>();

            // OutputDtos to responses.
            this.CreateMap<UserOutputDto, UserResponse>();
            this.CreateMap<Result<IEnumerable<UserOutputDto>>, Result<IEnumerable<UserResponse>>>();
        }
    }
}