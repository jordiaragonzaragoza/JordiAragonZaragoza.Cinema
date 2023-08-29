namespace JordiAragon.Cinema.Presentation.WebApi.FunctionalTests.Common
{
    using Xunit;

    [CollectionDefinition(nameof(SharedTestCollection))]
    public class SharedTestCollection : ICollectionFixture<FunctionalTestsFixture<Program>>
    {
    }
}