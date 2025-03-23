namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Presentation.HttpRestfulApi.V2
{
    using Ardalis.Result;
    using AutoMapper;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Movie.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Movie.Responses;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;

    public sealed class MoviesMapper : Profile
    {
        public MoviesMapper()
        {
            // Requests to queries or commands.
            this.CreateMap<GetMoviesRequest, GetMoviesQuery>();

            // ReadModels to responses.
            this.CreateMap<MovieReadModel, MovieResponse>();
            this.CreateMap<Result<MovieReadModel>, Result<MovieResponse>>();

            this.CreateMap<PaginatedCollectionOutputDto<MovieReadModel>, PaginatedCollectionResponse<MovieResponse>>();
            this.CreateMap<Result<PaginatedCollectionOutputDto<MovieReadModel>>, Result<PaginatedCollectionResponse<MovieResponse>>>();
        }
    }
}