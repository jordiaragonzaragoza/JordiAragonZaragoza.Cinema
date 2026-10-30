namespace JordiAragonZaragoza.Cinema.SharedKernel
{
    public static class Constants
    {
        public const string PostgresServer = "PostgresServer";

        public const string PostgresImage = "postgres";

        public const string PostgresImageTag = "18.6-alpine";

        public const string PgAdminTag = "9.18.0";

        public const string SeqServer = "SeqServer";

        public const string RedisCache = "RedisCache";

        public const string RedisImageTag = "8.10.2-alpine";

        public const string RedisInsightImageTag = "3.4";

        public const string Keycloak = "Keycloak";

        public const string KeycloakImageTag = "26.8";

        public const string KeycloakRealm = "jordiaragonzaragoza.cinema";

        public const string KeycloakAudience = "jordiaragonzaragoza.cinema.api";

        public const string ReservationApiCommand = "ReservationApiCommand";

        public const string ReservationWorkerReactor = "ReservationWorkerReactor";

        public const string ReservationApiQuery = "ReservationApiQuery";

        public const string ReservationMcpGateway = "ReservationMcpGateway";

        public const string ReservationWorkerProjector = "ReservationWorkerProjector";

        public const string ReservationWorkerSeeder = "ReservationWorkerSeeder";

        public const string ReservationWorkerReadModelMigrator = "ReservationWorkerReadModelMigrator";

        public const string ReservationBusinessModelStore = "ReservationBusinessModelStore";

        public const string ReservationReadModelStore = "ReservationReadModelStore";

        public const string KurrentDbImage = "kurrentplatform/kurrentdb";

        public const string KurrentDbArmImageTag = "26.2.3-experimental-arm64-10.0-noble";

        public const string KurrentDbImageTag = "26.2.3";
    }
}