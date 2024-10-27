namespace JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.Common
{
    using Xunit;

    [CollectionDefinition(nameof(SharedTestCollection))]
    public sealed class SharedTestCollection : ICollectionFixture<FunctionalTestsFixture<Program>>
    {
    }
}