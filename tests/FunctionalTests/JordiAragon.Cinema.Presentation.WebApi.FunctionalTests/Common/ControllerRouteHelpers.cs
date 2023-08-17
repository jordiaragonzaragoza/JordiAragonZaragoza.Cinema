namespace JordiAragon.Cinema.Presentation.WebApi.FunctionalTests.Common
{
    using System;
    using System.Web;
    using Microsoft.AspNetCore.Mvc;

    public static class ControllerRouteHelpers
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

        public static string GetControllerBasePath<TController>()
            where TController : ControllerBase
        {
            var controllerType = typeof(TController);
            var apiVersionAttribute = controllerType.GetCustomAttributes(typeof(ApiVersionAttribute), inherit: false)[0] as ApiVersionAttribute;

            var version = apiVersionAttribute.Versions[0].ToString(); // Assuming one version
            var controllerName = controllerType.Name;
            if (controllerName.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
            {
                controllerName = controllerName.Substring(0, controllerName.Length - "Controller".Length);
            }

            return $"api/v{version[0]}/{controllerName.ToLowerInvariant()}"; // Use only the major version
        }
    }
}