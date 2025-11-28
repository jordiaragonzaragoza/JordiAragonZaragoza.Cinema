namespace JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Api.Command.Common
{
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query;

    using Xunit;

    [CollectionDefinition(nameof(ApiCommandSharedTestCollection))]
    public sealed class ApiCommandSharedTestCollection : ICollectionFixture<FunctionalTestsFixture<Program>>
    {
    }
}