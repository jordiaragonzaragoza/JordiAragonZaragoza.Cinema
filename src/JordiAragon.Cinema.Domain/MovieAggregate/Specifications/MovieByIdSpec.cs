namespace JordiAragon.Cinema.Domain.MovieAggregate.Specifications
{
    using Ardalis.Specification;

    public class MovieByIdSpec : SingleResultSpecification<Movie>
    {
        public MovieByIdSpec(MovieId movieId)
        {
            this.Query
                .Where(movie => movie.Id == movieId);
        }
    }
}