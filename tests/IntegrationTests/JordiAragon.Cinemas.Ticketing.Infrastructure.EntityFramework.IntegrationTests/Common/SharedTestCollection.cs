namespace JordiAragon.Cinemas.Ticketing.Infrastructure.EntityFramework.IntegrationTests.Common
{
    using Xunit;

    [CollectionDefinition(nameof(SharedTestCollection))]
    public class SharedTestCollection : ICollectionFixture<IntegrationTestsFixture>
    {
    }
}