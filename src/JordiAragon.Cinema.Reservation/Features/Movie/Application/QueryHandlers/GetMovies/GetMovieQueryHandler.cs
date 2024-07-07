namespace JordiAragon.Cinema.Reservation.Movie.Application.QueryHandlers.GetMovies
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using AutoMapper;
    using JordiAragon.Cinema.Reservation.Movie.Application.Contracts.Queries;
    using JordiAragon.Cinema.Reservation.Movie.Application.Contracts.ReadModels;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.Repositories;

    // TODO: Temporal. Move. This query is part of other bounded context and will use read models. (Catalog)
    public sealed class GetMovieQueryHandler : IQueryHandler<GetMoviesQuery, IEnumerable<MovieOutputDto>>
    {
        private readonly IReadListRepository<Movie, MovieId> movieRepository;
        private readonly IMapper mapper;

        public GetMovieQueryHandler(
            IReadListRepository<Movie, MovieId> movieRepository,
            IMapper mapper)
        {
            this.movieRepository = movieRepository;
            this.mapper = mapper;
        }

        public async Task<Result<IEnumerable<MovieOutputDto>>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
        {
            var projects = await this.movieRepository.ListAsync(cancellationToken);
            if (!projects.Any())
            {
                return Result.NotFound($"{nameof(Movie)}/s not found.");
            }

            return Result.Success(this.mapper.Map<IEnumerable<MovieOutputDto>>(projects));
        }
    }
}