namespace JordiAragon.Cinemas.Ticketing.Domain.UnitTests.Features.Auditorium.TestUtils
{
    using JordiAragon.Cinemas.Ticketing.Domain.UnitTests.TestUtils.Constants;
    using JordiAragon.Cinemas.Ticketing.Domain.AuditoriumAggregate;

    public static class CreateAuditoriumUtils
    {
        public static Auditorium Create()
            => Auditorium.Create(
                Constants.Auditorium.Id,
                Constants.Auditorium.Rows,
                Constants.Auditorium.SeatsPerRow);
    }
}