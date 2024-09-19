namespace JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.Common
{
    using System;
    using System.Web;
    using Ardalis.GuardClauses;

    public static class EndpointRouteHelpers
    {
        public static Uri BuildUriWithQueryParameters(string basePath, params (string Key, string Value)[] queryParams)
        {
            Guard.Against.Null(queryParams, nameof(queryParams));

            var uriBuilder = new UriBuilder
            {
                Path = basePath,
            };

            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            foreach (var (key, value) in queryParams)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    query[key] = value;
                }
            }

            uriBuilder.Query = query.ToString();

            return uriBuilder.Uri;
        }
    }
}