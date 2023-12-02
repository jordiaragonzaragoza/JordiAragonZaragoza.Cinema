namespace JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.WebApi.Common
{
    using Xunit;

    [CollectionDefinition(nameof(SharedTestCollection))]
    public class SharedTestCollection : ICollectionFixture<FunctionalTestsFixture<Program>>
    {
    }
}