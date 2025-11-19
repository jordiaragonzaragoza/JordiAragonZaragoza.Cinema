namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Controllers.V2.Auditorium
{
    using System;
    using System.Collections.Generic;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Auditorium.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;

    public static class AuditoriumsMapper
    {
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
    }
}