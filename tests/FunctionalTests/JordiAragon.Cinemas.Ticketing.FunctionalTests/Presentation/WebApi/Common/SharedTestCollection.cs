namespace JordiAragon.Cinemas.Ticketing.FunctionalTests.Presentation.WebApi.Common
{
    using Xunit;

    [CollectionDefinition(nameof(SharedTestCollection))]
    public class SharedTestCollection : ICollectionFixture<FunctionalTestsFixture<Program>>
    {
    }
}