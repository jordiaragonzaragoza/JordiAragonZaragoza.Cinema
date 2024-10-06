namespace JordiAragon.Cinema
{
    using Aspire.Hosting;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1118:Utility classes should not have public constructors", Justification = "Program class should not have a protected constructor or the static keyword because is used for functional and integration test.")]
    public sealed class Program
    {
        public static void Main(string[] args)
        {
              var builder = DistributedApplication.CreateBuilder(args);

              var postgresServer = builder.AddPostgres("PostgresServer")
                                          .WithImageTag("15.1-alpine")
                                          .WithDataBindMount("../../containers/postgres/data");
                                          ////.WithPgAdmin();

              var reservationBusinessModelDb = postgresServer.AddDatabase("JordiAragonCinemaReservationBusinessModelStore");
              var reservationReadModelDb = postgresServer.AddDatabase("JordiAragonCinemaReservationReadModelStore");

              builder.AddContainer("EventStoreDbServer", "eventstore/eventstore", "23.10.1-alpha-arm64v8")
                     .WithEnvironment("EVENTSTORE_CLUSTER_SIZE", "1")
                     .WithEnvironment("EVENTSTORE_RUN_PROJECTIONS", "All")
                     .WithEnvironment("EVENTSTORE_START_STANDARD_PROJECTIONS", "true")
                     .WithEnvironment("EVENTSTORE_INSECURE", "true")
                     .WithEnvironment("EVENTSTORE_ENABLE_EXTERNAL_TCP", "true")
                     .WithEnvironment("EVENTSTORE_ENABLE_ATOM_PUB_OVER_HTTP", "true")
                     .WithBindMount("../../containers/eventstore/data/", "/var/lib/eventstore")
                     .WithBindMount("../../containers/eventstore/logs/", "/var/log/eventstore")
                     .WithEndpoint(2113, 2113, scheme: "https");

              var seq = builder.AddSeq("seq", port: 5341)
                                     .WithDataBindMount("../../containers/seq/data")
                                     .ExcludeFromManifest();

              builder.AddProject<Projects.JordiAragon_Cinema_Reservation>("JordiAragonCinemaReservation")
                     .WithReference(reservationBusinessModelDb)
                     .WithReference(reservationReadModelDb)
                     .WithReference(seq)
                     .WithHttpsEndpoint(7001, 7001, isProxied: false);

              builder.Build().Run();
        }
    }
}