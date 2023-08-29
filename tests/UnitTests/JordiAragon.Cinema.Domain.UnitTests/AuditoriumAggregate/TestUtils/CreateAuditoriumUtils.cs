namespace JordiAragon.Cinema.Domain.UnitTests.Features.Auditorium.TestUtils
{
    using JordiAragon.Cinema.Domain.UnitTests.TestUtils.Constants;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;

    public static class CreateAuditoriumUtils
    {
        public static Auditorium Create()
            => Auditorium.Create(
                Constants.Auditorium.Id,
                Constants.Auditorium.Rows,
                Constants.Auditorium.SeatsPerRow);
    }
}