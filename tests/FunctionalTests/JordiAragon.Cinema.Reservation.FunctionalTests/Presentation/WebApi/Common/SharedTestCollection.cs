﻿namespace JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.WebApi.Common
{
    using Xunit;

    [CollectionDefinition(nameof(SharedTestCollection))]
    public sealed class SharedTestCollection : ICollectionFixture<FunctionalTestsFixture<Program>>
    {
    }
}