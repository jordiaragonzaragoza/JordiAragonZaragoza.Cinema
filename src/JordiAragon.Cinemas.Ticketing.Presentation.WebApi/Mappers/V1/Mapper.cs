namespace JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Mappers.V1
{
    using AutoMapper;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Controllers.V1;

    public class Mapper : Profile
    {
        public Mapper()
        {
            this.MapAuditorium();
            this.MapMovie();
        }
    }
}
