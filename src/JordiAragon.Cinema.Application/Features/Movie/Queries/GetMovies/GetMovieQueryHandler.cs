namespace JordiAragon.Cinema.Application.Features.Movie.Queries.GetMovies
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using AutoMapper;
    using JordiAragon.Cinema.Application.Contracts.Features.Movie.Queries;
    using JordiAragon.Cinema.Domain.MovieAggregate;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public class GetMovieQueryHandler : IQueryHandler<GetMoviesQuery, IEnumerable<MovieOutputDto>>
    {
        private readonly IReadRepository<Movie> movieRepository;
        private readonly IMapper mapper;

        public GetMovieQueryHandler(
            IReadRepository<Movie> movieRepository,
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