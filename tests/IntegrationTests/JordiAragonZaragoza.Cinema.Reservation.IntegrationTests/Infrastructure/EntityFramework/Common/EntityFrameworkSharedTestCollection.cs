namespace JordiAragonZaragoza.Cinema.Reservation.IntegrationTests.Infrastructure.EntityFramework.Common
{
    using Xunit;

    [CollectionDefinition(nameof(EntityFrameworkSharedTestCollection))]
    public sealed class EntityFrameworkSharedTestCollection : ICollectionFixture<IntegrationTestsFixture>
    {
    }
}