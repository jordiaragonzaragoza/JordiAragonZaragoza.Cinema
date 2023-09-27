namespace JordiAragon.Cinemas.Ticketing.Application
{
    using AutoMapper;
    using JordiAragon.Cinemas.Ticketing.Application.Auditorium.Queries;
    using JordiAragon.Cinemas.Ticketing.Application.Movie.Queries;
    using JordiAragon.Cinemas.Ticketing.Application.Showtime.Queries;

    public class Mapper : Profile
    {
        public Mapper()
        {
            this.MapAuditorium();
            this.MapMovie();
            this.MapShowtime();
        }
    }
}
