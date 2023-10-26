namespace JordiAragon.Cinemas.Ticketing.FunctionalTests.Presentation.WebApi.Common
{
    using System;
    using System.Web;

    public static class EndpointRouteHelpers
    {
        public static string BuildUriWithQueryParameters(string basePath, params (string Key, string Value)[] queryParams)
        {
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

            return uriBuilder.Uri.PathAndQuery;
        }
    }
}