namespace JordiAragonZaragoza.Cinema
{
    using Aspire.Hosting;
    using JordiAragonZaragoza.Cinema.SharedKernel;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1118:Utility classes should not have public constructors", Justification = "Program class should not have a protected constructor or the static keyword because is used for functional and integration test.")]
    public sealed class Program
    {
        public static void Main(string[] args)
        {
            var builder = DistributedApplication.CreateBuilder(args);

              /*var postgresServer = builder.AddPostgres(Constants.PostgresServer)
                                          .WithImageTag("15.1-alpine")
                                          .WithDataBindMount("../../containers/postgres/data")
                                          .WithPgAdmin();

              var reservationReadModelDb = postgresServer.AddDatabase(Constants.JordiAragonZaragozaCinemaReservationReadModelStore);*/

            /*var seq = builder.AddSeq(Constants.SeqServer, port: 5341)
                                   .WithDataBindMount("../../containers/seq/data")
                                   .ExcludeFromManifest();*/

            var kurrentdb = builder.AddKurrentDB(Constants.JordiAragonZaragozaCinemaReservationBusinessModelStore, 22113);

            builder.AddProject<Projects.JordiAragonZaragoza_Cinema_Reservation_Api_Command>(Constants.JordiAragonZaragozaCinemaReservationApiCommand)
                     .WithReference(kurrentdb)
                     .WaitFor(kurrentdb);

            builder.Build().Run();
        }
    }
}