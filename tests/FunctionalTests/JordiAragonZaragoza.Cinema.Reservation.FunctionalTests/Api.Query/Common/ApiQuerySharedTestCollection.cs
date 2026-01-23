namespace JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Api.Query.Common
{
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query;

    using Xunit;

    [CollectionDefinition(nameof(ApiQuerySharedTestCollection))]
    public sealed class ApiQuerySharedTestCollection : ICollectionFixture<FunctionalTestsFixture<Program>>
    {
    }
}