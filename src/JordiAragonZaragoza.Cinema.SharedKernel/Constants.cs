namespace JordiAragonZaragoza.Cinema.SharedKernel
{
    public static class Constants
    {
        public const string PostgresServer = "PostgresServer";

        public const string SeqServer = "SeqServer";

        public const string RedisCache = "RedisCache";

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

        public const string KurrentDbArmImageTag = "26.0.0-experimental-arm64-10.0-noble";

        public const string KurrentDbImageTag = "26.0.0";

        public const string PostgresImage = "postgres";

        public const string PostgresImageTag = "18.1-alpine";

        public const string RedisImageTag = "8.6.2";
    }
}