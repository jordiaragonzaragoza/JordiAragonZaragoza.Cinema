namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Presentation.HttpRestfulApi.V2
{
    using System;
    using System.Collections.Generic;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Auditorium.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Auditorium.Responses;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;

    public static class AuditoriumsMapper
    {
        public static GetAuditoriumsQuery ToQuery(this GetAuditoriumsRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            return new GetAuditoriumsQuery(
                request.PageNumber ?? 1,
                request.PageSize ?? 10);
        }

        public static Result<PaginatedCollectionResponse<AuditoriumResponse>> ToResponse(
            this Result<PaginatedCollectionOutputDto<AuditoriumReadModel>> result)
        {
            ArgumentNullException.ThrowIfNull(result);

            return result.Map(paginatedCollection =>
                new PaginatedCollectionResponse<AuditoriumResponse>(
                    paginatedCollection.ActualPage,
                    paginatedCollection.TotalPages,
                    paginatedCollection.TotalItems,
                    paginatedCollection.Items.ToResponse()));
        }

        private static IEnumerable<AuditoriumResponse> ToResponse(
            this IEnumerable<AuditoriumReadModel> auditoriumReadModels)
        {
            ArgumentNullException.ThrowIfNull(auditoriumReadModels);

            return ToResponseIterator(auditoriumReadModels);
        }

        private static IEnumerable<AuditoriumResponse> ToResponseIterator(
            IEnumerable<AuditoriumReadModel> auditoriumReadModels)
        {
            foreach (var auditoriumReadModel in auditoriumReadModels)
            {
                yield return new AuditoriumResponse(
                    auditoriumReadModel.Id,
                    auditoriumReadModel.Name);
            }
        }

        /*public AuditoriumsMapper()
        {
            // Requests to Queries or commands
            this.CreateMap<GetAuditoriumsRequest, GetAuditoriumsQuery>();

            // OutputDtos to responses.
            this.CreateMap<SeatOutputDto, SeatResponse>();
            this.CreateMap<Result<IEnumerable<SeatOutputDto>>, Result<IEnumerable<SeatResponse>>>();

            this.CreateMap<AuditoriumReadModel, AuditoriumResponse>();
            this.CreateMap<Result<AuditoriumReadModel>, Result<AuditoriumResponse>>();

            this.CreateMap<PaginatedCollectionOutputDto<AuditoriumReadModel>, PaginatedCollectionResponse<AuditoriumResponse>>();
            this.CreateMap<Result<PaginatedCollectionOutputDto<AuditoriumReadModel>>, Result<PaginatedCollectionResponse<AuditoriumResponse>>>();

            this.CreateMap<SeatReadModel, SeatResponse>();

            this.CreateMap<AvailableSeatReadModel, SeatResponse>()
                .ForCtorParam(nameof(AvailableSeatReadModel.Id), opt => opt.MapFrom(src => src.SeatId));
            this.CreateMap<Result<IEnumerable<AvailableSeatReadModel>>, Result<IEnumerable<SeatResponse>>>();
        }*/
    }
}