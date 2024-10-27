namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Presentation.HttpRestfulApi.V2
{
    using Ardalis.Result;
    using AutoMapper;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;

    public sealed class ShowtimesMapper : Profile
    {
        public ShowtimesMapper()
        {
            // Requests to queries or commands.
            this.CreateMap<ScheduleShowtimeRequest, ScheduleShowtimeCommand>();
            this.CreateMap<ReserveSeatsRequest, ReserveSeatsCommand>();
            this.CreateMap<GetShowtimesRequest, GetShowtimesQuery>();
            this.CreateMap<GetShowtimeRequest, GetShowtimeQuery>();
            this.CreateMap<GetShowtimeTicketsRequest, GetShowtimeTicketsQuery>();

            // OutputDtos to responses.
            this.CreateMap<TicketOutputDto, TicketResponse>();
            this.CreateMap<Result<TicketOutputDto>, Result<TicketResponse>>();

            this.CreateMap<TicketReadModel, TicketResponse>();
            this.CreateMap<Result<TicketReadModel>, Result<TicketResponse>>();
            this.CreateMap<PaginatedCollectionOutputDto<TicketReadModel>, PaginatedCollectionResponse<TicketResponse>>();
            this.CreateMap<Result<PaginatedCollectionOutputDto<TicketReadModel>>, Result<PaginatedCollectionResponse<TicketResponse>>>();

            this.CreateMap<ShowtimeReadModel, ShowtimeResponse>();
            this.CreateMap<Result<ShowtimeReadModel>, Result<ShowtimeResponse>>();

            this.CreateMap<PaginatedCollectionOutputDto<ShowtimeReadModel>, PaginatedCollectionResponse<ShowtimeResponse>>();
            this.CreateMap<Result<PaginatedCollectionOutputDto<ShowtimeReadModel>>, Result<PaginatedCollectionResponse<ShowtimeResponse>>>();
        }
    }
}