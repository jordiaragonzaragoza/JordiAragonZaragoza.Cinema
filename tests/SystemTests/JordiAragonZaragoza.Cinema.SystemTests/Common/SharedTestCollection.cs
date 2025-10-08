namespace JordiAragonZaragoza.Cinema.SystemTests.Common
{
    using Xunit;

    [CollectionDefinition(nameof(SharedTestCollection))]
    public sealed class SharedTestCollection : ICollectionFixture<SystemTestsFixture>
    {
    }
}