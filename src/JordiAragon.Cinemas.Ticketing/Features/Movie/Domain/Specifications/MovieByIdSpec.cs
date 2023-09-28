namespace JordiAragon.Cinemas.Ticketing.Movie.Domain.Specifications
{
    using Ardalis.GuardClauses;
    using Ardalis.Specification;

    public class MovieByIdSpec : SingleResultSpecification<Movie>
    {
        public MovieByIdSpec(MovieId movieId)
        {
            Guard.Against.Null(movieId);

            this.Query
                .Where(movie => movie.Id == movieId);
        }
    }
}