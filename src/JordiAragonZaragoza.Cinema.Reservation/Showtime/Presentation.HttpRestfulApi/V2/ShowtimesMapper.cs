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
            this.CreateMap<GetShowtimeReservationsRequest, GetShowtimeReservationsQuery>();

            // OutputDtos to responses.
            this.CreateMap<ReservationOutputDto, ReservationResponse>();
            this.CreateMap<Result<ReservationOutputDto>, Result<ReservationResponse>>();

            this.CreateMap<ReservationReadModel, ReservationResponse>();
            this.CreateMap<Result<ReservationReadModel>, Result<ReservationResponse>>();
            this.CreateMap<PaginatedCollectionOutputDto<ReservationReadModel>, PaginatedCollectionResponse<ReservationResponse>>();
            this.CreateMap<Result<PaginatedCollectionOutputDto<ReservationReadModel>>, Result<PaginatedCollectionResponse<ReservationResponse>>>();

            this.CreateMap<ShowtimeReadModel, ShowtimeResponse>();
            this.CreateMap<Result<ShowtimeReadModel>, Result<ShowtimeResponse>>();

            this.CreateMap<PaginatedCollectionOutputDto<ShowtimeReadModel>, PaginatedCollectionResponse<ShowtimeResponse>>();
            this.CreateMap<Result<PaginatedCollectionOutputDto<ShowtimeReadModel>>, Result<PaginatedCollectionResponse<ShowtimeResponse>>>();
        }
    }
}