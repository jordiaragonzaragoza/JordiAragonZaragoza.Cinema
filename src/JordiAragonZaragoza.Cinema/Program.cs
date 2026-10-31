namespace JordiAragonZaragoza.Cinema
{
    using Aspire.Hosting;
    using Aspire.Hosting.ApplicationModel;
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
                kurrentdb.WithDataVolume()
                         .WithLifetime(ContainerLifetime.Persistent);
            }
            else
            {
                kurrentdb.WithContainerRuntimeArgs("--tmpfs", "/var/lib/kurrentdb")
                         .WithLifetime(ContainerLifetime.Session);
            }

            var kurrentdbSeeder = builder.AddProject<Projects.JordiAragonZaragoza_Cinema_Reservation_Worker_Seeder>(Constants.ReservationWorkerSeeder)
                                          .WithReference(kurrentdb)
                                          .WaitFor(kurrentdb);

            var postgresServer = builder.AddPostgres(Constants.PostgresServer)
                                        .WithImageTag(Constants.PostgresImageTag)
                                        .WithPgAdmin(pgAdmin => pgAdmin.WithImageTag(Constants.PgAdminTag));

            if (!IsSystemTesting(builder))
            {
                postgresServer.WithDataVolume()
                              .WithLifetime(ContainerLifetime.Persistent);
            }
            else
            {
                postgresServer.WithContainerRuntimeArgs("--tmpfs", "/var/lib/postgresql")
                              .WithLifetime(ContainerLifetime.Session);
            }

            var cache = builder.AddRedis(Constants.RedisCache)
                               .WithImageTag(Constants.RedisImageTag)
                               .WithRedisInsight(redisInsight => redisInsight.WithImageTag(Constants.RedisInsightImageTag));

            if (!IsSystemTesting(builder))
            {
                cache.WithDataVolume()
                     .WithLifetime(ContainerLifetime.Persistent);
            }
            else
            {
                cache.WithContainerRuntimeArgs("--tmpfs", "/data")
                     .WithLifetime(ContainerLifetime.Session);
            }

            var reservationReadModelDb = postgresServer.AddDatabase(Constants.ReservationReadModelStore);

            var reservationWorkerReadModelMigrator = builder.AddProject<Projects.JordiAragonZaragoza_Cinema_Reservation_Worker_ReadModelMigrator>(Constants.ReservationWorkerReadModelMigrator)
                                                          .WithReference(reservationReadModelDb)
                                                          .WaitFor(postgresServer);

            var keycloak = builder.AddKeycloak(Constants.Keycloak, 8080)
                                  .WithOtlpExporter()
                                  .WithImageTag(Constants.KeycloakImageTag)
                                  .WithRealmImport("../../containers/keycloak");

            if (!IsSystemTesting(builder))
            {
                keycloak.WithDataVolume()
                        .WithLifetime(ContainerLifetime.Persistent);
            }
            else
            {
                keycloak.WithContainerRuntimeArgs("--tmpfs", "/opt/keycloak/data")
                        .WithLifetime(ContainerLifetime.Session);
            }

            var keycloakAuthority = ReferenceExpression.Create($"{keycloak.GetEndpoint("http").Property(EndpointProperty.Url)}/realms/{Constants.KeycloakRealm}");

            var reservationApiCommand = builder.AddProject<Projects.JordiAragonZaragoza_Cinema_Reservation_Api_Command>(Constants.ReservationApiCommand)
                     .WithReference(kurrentdb)
                     .WaitForCompletion(kurrentdbSeeder)
                     .WithReference(reservationReadModelDb)
                     .WaitForCompletion(reservationWorkerReadModelMigrator)
                     .WithReference(cache)
                     .WithReference(keycloak)
                     .WaitFor(keycloak)
                     .WithEnvironment("Authentication__Schemes__Bearer__Authority", keycloakAuthority)
                     .WithEnvironment("Authentication__Claims__UserIdClaimType", "sub");

            var reservationApiQuery = builder.AddProject<Projects.JordiAragonZaragoza_Cinema_Reservation_Api_Query>(Constants.ReservationApiQuery)
                     .WithReference(reservationReadModelDb)
                     .WaitForCompletion(reservationWorkerReadModelMigrator)
                     .WithReference(cache)
                     .WithReference(keycloak)
                     .WaitFor(keycloak)
                     .WithEnvironment("Authentication__Schemes__Bearer__Authority", keycloakAuthority)
                     .WithEnvironment("Authentication__Claims__UserIdClaimType", "sub");

            builder.AddProject<Projects.JordiAragonZaragoza_Cinema_Reservation_Worker_Projector>(Constants.ReservationWorkerProjector)
                                          .WithReference(kurrentdb)
                                          .WaitFor(kurrentdb)
                                          .WithReference(reservationReadModelDb)
                                          .WaitForCompletion(reservationWorkerReadModelMigrator)
                                          .WithReference(cache);

            builder.AddProject<Projects.JordiAragonZaragoza_Cinema_Reservation_Worker_Reactor>(Constants.ReservationWorkerReactor)
                     .WithReference(kurrentdb)
                     .WaitForCompletion(kurrentdbSeeder)

                     // TODO: Temporal coupling: Projections required to execute for some batch-job policies.
                     .WithReference(reservationReadModelDb)
                     .WaitForCompletion(reservationWorkerReadModelMigrator)
                     .WithReference(cache);

            builder.AddProject<Projects.JordiAragonZaragoza_Cinema_Reservation_Mcp_Gateway>(Constants.ReservationMcpGateway)
                     .WithReference(reservationApiQuery)
                     .WaitFor(reservationApiQuery)
                     .WithReference(reservationApiCommand)
                     .WaitFor(reservationApiCommand);

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