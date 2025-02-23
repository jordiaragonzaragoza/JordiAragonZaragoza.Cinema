namespace JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.Common
{
    using System;
    using System.Web;

    public static class EndpointRouteHelpers
    {
        public static Uri BuildUriWithQueryParameters(string basePath, params (string Key, string Value)[] queryParams)
        {
            ArgumentNullException.ThrowIfNull(basePath, nameof(basePath));
            ArgumentNullException.ThrowIfNull(queryParams, nameof(queryParams));

            foreach (var (key, value) in queryParams)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    basePath = basePath.Replace($"{{{key}}}", value, StringComparison.OrdinalIgnoreCase);
                }
            }

            var uriBuilder = new UriBuilder
            {
                Path = basePath,
            };

            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            foreach (var (key, value) in queryParams)
            {
                if (!string.IsNullOrWhiteSpace(value) && !basePath.Contains($"{{{key}}}", StringComparison.OrdinalIgnoreCase))
                {
                    query[key] = value;
                }
            }

            uriBuilder.Query = query.ToString();

            return uriBuilder.Uri;
        }
    }
}