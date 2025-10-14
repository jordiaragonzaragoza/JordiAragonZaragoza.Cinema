namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Presentation.HttpRestfulApi.V2
{
    using System;
    using System.Collections.Generic;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Movie.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Movie.Responses;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;

    public static class MoviesMapper
    {
        public static GetMoviesQuery ToQuery(this GetMoviesRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            return new GetMoviesQuery(
                request.PageNumber ?? 1,
                request.PageSize ?? 10);
        }

        public static Result<PaginatedCollectionResponse<MovieResponse>> ToResponse(
            this Result<PaginatedCollectionOutputDto<MovieReadModel>> result)
        {
            ArgumentNullException.ThrowIfNull(result);

            return result.Map(paginatedCollection =>
                new PaginatedCollectionResponse<MovieResponse>(
                    paginatedCollection.ActualPage,
                    paginatedCollection.TotalPages,
                    paginatedCollection.TotalItems,
                    paginatedCollection.Items.ToResponse()));
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