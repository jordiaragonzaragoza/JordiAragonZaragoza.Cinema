namespace JordiAragon.Cinemas.Ticketing.Presentation.WebApi.FunctionalTests.Common
{
    using Xunit;

    [CollectionDefinition(nameof(SharedTestCollection))]
    public class SharedTestCollection : ICollectionFixture<FunctionalTestsFixture<Program>>
    {
    }
}