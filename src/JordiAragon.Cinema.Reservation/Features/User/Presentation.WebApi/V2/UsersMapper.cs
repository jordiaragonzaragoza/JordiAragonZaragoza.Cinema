namespace JordiAragon.Cinema.Reservation.User.Presentation.WebApi.V2
{
    using System.Collections.Generic;
    using Ardalis.Result;
    using AutoMapper;
    using JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Auditorium.Responses;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Responses;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.User.Requests;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.User.Responses;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragon.Cinema.Reservation.User.Application.Contracts.ReadModels;
    using JordiAragon.SharedKernel.Application.Contracts;
    using JordiAragon.SharedKernel.Presentation.WebApi.Contracts;

    public class UsersMapper : Profile
    {
        public UsersMapper()
        {
            // Requests to queries or commands.
            this.CreateMap<UserTicketRequest, GetUserTicketQuery>();
            this.CreateMap<UserTicketsRequest, GetUserTicketsQuery>();

            // OutputDtos to responses.
            this.CreateMap<UserOutputDto, UserResponse>();
            this.CreateMap<Result<IEnumerable<UserOutputDto>>, Result<IEnumerable<UserResponse>>>();

            this.CreateMap<SeatReadModel, SeatResponse>();
            this.CreateMap<TicketReadModel, TicketResponse>();
            this.CreateMap<Result<TicketReadModel>, Result<TicketResponse>>();
            this.CreateMap<PaginatedCollectionOutputDto<TicketReadModel>, PaginatedCollectionResponse<TicketResponse>>();
            this.CreateMap<Result<PaginatedCollectionOutputDto<TicketReadModel>>, Result<PaginatedCollectionResponse<TicketResponse>>>();
        }
    }
}