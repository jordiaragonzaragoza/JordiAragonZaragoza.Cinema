namespace JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Mappers.V2
{
    using AutoMapper;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Controllers.V2;

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
