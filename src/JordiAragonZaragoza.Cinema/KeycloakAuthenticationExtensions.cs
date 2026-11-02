namespace JordiAragonZaragoza.Cinema
{
    using Aspire.Hosting;
    using Aspire.Hosting.ApplicationModel;
    using JordiAragonZaragoza.Cinema.SharedKernel;

    /// <summary>
    /// Describes the local Keycloak to the APIs. In cloud environments the same keys come from
    /// appsettings / environment variables pointing to Entra ID; the API code does not change.
    /// </summary>
    internal static class KeycloakAuthenticationExtensions
    {
        public static IResourceBuilder<ProjectResource> WithKeycloakAuthentication(
            this IResourceBuilder<ProjectResource> builder,
            IResourceBuilder<KeycloakResource> keycloak)
        {
            var authority = ReferenceExpression.Create(
                $"{keycloak.GetEndpoint("http").Property(EndpointProperty.Url)}/realms/{Constants.KeycloakRealm}");

            var oidc = ReferenceExpression.Create($"{authority}/protocol/openid-connect");

            return builder
                .WaitFor(keycloak)
                .WithEnvironment("Authentication__Schemes__Bearer__Authority", authority)
                .WithEnvironment("Authentication__Schemes__Bearer__Audience", Constants.KeycloakAudience)
                .WithEnvironment("Authentication__Claims__UserIdClaimType", "sub")
                .WithEnvironment("Swagger__OAuth2__AuthorizationUrl", ReferenceExpression.Create($"{oidc}/auth"))
                .WithEnvironment("Swagger__OAuth2__TokenUrl", ReferenceExpression.Create($"{oidc}/token"));
        }
    }
}
