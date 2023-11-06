namespace JordiAragon.Cinemas.Reservation.IntegrationTests.Infrastructure.EntityFramework.Common
{
    using Xunit;

    [CollectionDefinition(nameof(SharedTestCollection))]
    public class SharedTestCollection : ICollectionFixture<IntegrationTestsFixture>
    {
    }
}