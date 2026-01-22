namespace JordiAragonZaragoza.Cinema
{
    using Aspire.Hosting;
    using JordiAragonZaragoza.Cinema.SharedKernel;
    using Microsoft.Extensions.Configuration;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1118:Utility classes should not have public constructors", Justification = "Program class should not have a protected constructor or the static keyword because is used for functional and integration test.")]
    public sealed class Program
    {
        public static void Main(string[] args)
        {
            var builder = DistributedApplication.CreateBuilder(args);

            var kurrentdb = builder.AddKurrentDB(Constants.ReservationBusinessModelStore)
                                   .WithImageRegistry("docker.io")
                                   .WithImage(Constants.KurrentDbImage, Constants.KurrentDbArmImageTag);

            if (!IsSystemTesting(builder))
            {
                kurrentdb.WithDataVolume();
            }
            else
            {
                kurrentdb.WithContainerRuntimeArgs("--tmpfs", "/var/lib/kurrentdb");
            }

            var kurrentdbSeeder = builder.AddProject<Projects.JordiAragonZaragoza_Cinema_Reservation_Worker_Seeder>(Constants.ReservationWorkerSeeder)
                                          .WithReference(kurrentdb)
                                          .WaitFor(kurrentdb);

            builder.AddProject<Projects.JordiAragonZaragoza_Cinema_Reservation_Api_Command>(Constants.ReservationApiCommand)
                     .WithReference(kurrentdb)
                     .WaitForCompletion(kurrentdbSeeder);

            var postgresServer = builder.AddPostgres(Constants.PostgresServer)
                                        .WithImageTag(Constants.PostgresImageTag)
                                        .WithPgAdmin();

            if (!IsSystemTesting(builder))
            {
                postgresServer.WithDataVolume();
            }
            else
            {
                postgresServer.WithContainerRuntimeArgs("--tmpfs", "/var/lib/postgresql/data");
            }

            var reservationReadModelDb = postgresServer.AddDatabase(Constants.ReservationReadModelStore);

            var reservationWorkerReadModelMigrator = builder.AddProject<Projects.JordiAragonZaragoza_Cinema_Reservation_Worker_ReadModelMigrator>(Constants.ReservationWorkerReadModelMigrator)
                                                          .WithReference(reservationReadModelDb)
                                                          .WaitFor(postgresServer);

            builder.AddProject<Projects.JordiAragonZaragoza_Cinema_Reservation_Api_Query>(Constants.ReservationApiQuery)
                     .WithReference(reservationReadModelDb)
                     .WaitForCompletion(reservationWorkerReadModelMigrator);

            builder.AddProject<Projects.JordiAragonZaragoza_Cinema_Reservation_Worker_Projector>(Constants.ReservationWorkerProjector)
                                          .WithReference(kurrentdb)
                                          .WaitFor(kurrentdb)
                                          .WithReference(reservationReadModelDb)
                                          .WaitForCompletion(reservationWorkerReadModelMigrator);

            builder.AddProject<Projects.JordiAragonZaragoza_Cinema_Reservation_Worker_Reactor>(Constants.ReservationWorkerReactor)
                     .WithReference(kurrentdb)
                     .WaitForCompletion(kurrentdbSeeder)

                     // TODO: Temporal coupling: Projections required to execute for some batch-job policies.
                     .WithReference(reservationReadModelDb)
                     .WaitForCompletion(reservationWorkerReadModelMigrator);

            /*var seq = builder.AddSeq(Constants.SeqServer, port: 5341)
                        .WithDataBindMount("../../containers/seq/data")
                        .ExcludeFromManifest();*/

            builder.Build().Run();
        }

        private static bool IsSystemTesting(IDistributedApplicationBuilder builder)
        {
            return builder.Configuration.GetValue<bool>("IsTesting");
        }
    }
}