namespace JordiAragonZaragoza.Cinema.SystemTests.Common
{
    /// <summary>
    /// Development user defined in the Keycloak realm import (users section).
    /// Development-only credentials: never reuse real ones here.
    /// </summary>
    public static class SystemTestUser
    {
        public const string UserName = "jordiaragonzaragoza";

        // Must match the credential declared in the realm JSON.
        public const string Password = "jordiaragonzaragoza";
    }
}
