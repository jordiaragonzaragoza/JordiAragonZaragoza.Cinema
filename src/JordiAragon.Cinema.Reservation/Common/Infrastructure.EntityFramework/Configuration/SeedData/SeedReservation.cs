namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration.SeedData
{
    using System;
    using System.Data.Common;
    using System.IO;
    using System.Reflection;

    public static class SeedReservation
    {
        public static void InitReservationBusinessModelData(DbConnection dbConnection)
        {
            var tableName = "__EFMigrationsHistory";
            var tableExists = TableExists(dbConnection, tableName);

            if (!tableExists)
            {
                var dbScript = GetEmbeddedResource("Common.Infrastructure.EntityFramework.Configuration.SeedData.ReservationBusinessModelData.sql");
                if (string.IsNullOrEmpty(dbScript))
                {
                    throw new InvalidOperationException($"Empty database script.");
                }

                ExecuteDbScript(dbConnection, dbScript);
            }
        }

        public static void InitReservationReadModelData(DbConnection dbConnection)
        {
            var tableName = "__EFMigrationsHistory";
            var tableExists = TableExists(dbConnection, tableName);

            if (!tableExists)
            {
                var dbScript = GetEmbeddedResource("Common.Infrastructure.EntityFramework.Configuration.SeedData.ReservationReadModelData.sql");
                if (string.IsNullOrEmpty(dbScript))
                {
                    throw new InvalidOperationException($"Empty database script.");
                }

                ExecuteDbScript(dbConnection, dbScript);
            }
        }

        private static void ExecuteDbScript(DbConnection dbConnection, string dbScript)
        {
            dbConnection.Open();

            using var dbCommand = dbConnection.CreateCommand();

            dbCommand.CommandText = dbScript;

            dbCommand.ExecuteNonQuery();

            dbConnection.Close();
        }

        private static bool TableExists(DbConnection dbConnection, string tableName)
        {
            dbConnection.Open();

            using var dbCommand = dbConnection.CreateCommand();

            dbCommand.CommandText = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}'";

            var count = dbCommand.ExecuteScalar() as int?;

            dbConnection.Close();

            return count > 0;
        }

        private static string GetEmbeddedResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fullResourceName = $"{assembly.GetName().Name}.{resourceName}";

            var resourceStream = assembly.GetManifestResourceStream(fullResourceName);

            if (resourceStream == null)
            {
                throw new InvalidOperationException($"Resource '{resourceName}' not found in assembly.");
            }

            using var reader = new StreamReader(resourceStream);
            return reader.ReadToEnd();
        }
    }
}