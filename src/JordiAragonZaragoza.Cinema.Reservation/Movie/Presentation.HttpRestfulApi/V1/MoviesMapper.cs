namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Presentation.HttpRestfulApi.V1
{
    using System;
    using System.Collections.Generic;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1.Movie.Responses;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;

    public static class MoviesMapper
    {
        public static Result<IEnumerable<MovieResponse>> ToResponse(
            this Result<PaginatedCollectionOutputDto<MovieReadModel>> result)
        {
            ArgumentNullException.ThrowIfNull(result);

            return result.Map(paginatedCollection => paginatedCollection.Items.ToResponse());
        }

        private static IEnumerable<MovieResponse> ToResponse(
            this IEnumerable<MovieReadModel> movieReadModels)
        {
            ArgumentNullException.ThrowIfNull(movieReadModels);

            return ToResponseIterator(movieReadModels);
        }

        private static IEnumerable<MovieResponse> ToResponseIterator(
            IEnumerable<MovieReadModel> movieReadModels)
        {
            foreach (var movieReadModel in movieReadModels)
            {
                yield return new MovieResponse(
                    movieReadModel.Id,
                    movieReadModel.Title,
                    movieReadModel.Runtime);
            }
        }
    }
}