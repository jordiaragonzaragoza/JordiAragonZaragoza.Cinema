namespace JordiAragon.Cinema.Presentation.WebApi.FunctionalTests.Common
{
    using System;
    using Microsoft.AspNetCore.Mvc;

    public static class ControllerBaseExtensions
    {
        public static string GetControllerBaseRoute<TController>()
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