namespace JordiAragon.Cinema.Reservation.IntegrationTests.Infrastructure.EntityFramework.Common
{
    using Xunit;

    [CollectionDefinition(nameof(SharedTestCollection))]
    public sealed class SharedTestCollection : ICollectionFixture<IntegrationTestsFixture>
    {
    }
}