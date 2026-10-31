namespace JordiAragonZaragoza.Cinema.SharedKernel.Presentation.HttpRestfulApi
{
    using System.Collections.Generic;
    using Asp.Versioning;

    /// <summary>
    /// Settings that vary from one API host to another. Everything else is shared.
    /// </summary>
    public sealed class ApiOptions
    {
        /// <summary>
        /// Gets the title shown in the Swagger documents.
        /// </summary>
        public required string Title { get; init; }

        /// <summary>
        /// Gets the Keycloak client used by Swagger UI.
        /// </summary>
        public string SwaggerClientId { get; init; } = "swagger-ui";

        /// <summary>
        /// Gets the OAuth2 scopes requested by Swagger UI. Add the API specific
        /// scope here (for example "reservation-api") when audiences are split.
        /// </summary>
        public IReadOnlyCollection<string> SwaggerScopes { get; init; } = ["openid", "profile"];

        /// <summary>
        /// Gets the API version assumed when the request does not specify one.
        /// </summary>
        public ApiVersion DefaultApiVersion { get; init; } = new ApiVersion(2);
    }
}
