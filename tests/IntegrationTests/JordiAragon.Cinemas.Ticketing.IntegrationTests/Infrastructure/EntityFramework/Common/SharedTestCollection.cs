namespace JordiAragon.Cinemas.Ticketing.IntegrationTests.Infrastructure.EntityFramework.Common
{
    using Xunit;

    [CollectionDefinition(nameof(SharedTestCollection))]
    public class SharedTestCollection : ICollectionFixture<IntegrationTestsFixture>
    {
    }
}