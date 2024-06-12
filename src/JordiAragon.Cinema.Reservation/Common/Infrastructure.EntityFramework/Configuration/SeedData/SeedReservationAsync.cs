namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration.SeedData
{
    using System;
    using System.Data.Common;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;

    public static class SeedReservationAsync
    {
        public static async Task InitReservationBusinessModelDataAsync(DbConnection dbConnection)
        {
            var tableName = "__EFMigrationsHistory";
            var tableExists = await TableExistsAsync(dbConnection, tableName);

            if (!tableExists)
            {
                var dbScript = GetEmbeddedResource("Common.Infrastructure.EntityFramework.Configuration.SeedData.ReservationBusinessModelData.sql");
                if (string.IsNullOrEmpty(dbScript))
                {
                    throw new InvalidOperationException($"Empty database script.");
                }

                await ExecuteDbScriptAsync(dbConnection, dbScript);
            }
        }

        public static async Task InitReservationReadModelDataAsync(DbConnection dbConnection)
        {
            var tableName = "__EFMigrationsHistory";
            var tableExists = await TableExistsAsync(dbConnection, tableName);

            if (!tableExists)
            {
                var dbScript = GetEmbeddedResource("Common.Infrastructure.EntityFramework.Configuration.SeedData.ReservationReadModelData.sql");
                if (string.IsNullOrEmpty(dbScript))
                {
                    throw new InvalidOperationException($"Empty database script.");
                }

                await ExecuteDbScriptAsync(dbConnection, dbScript);
            }
        }

        private static async Task ExecuteDbScriptAsync(DbConnection dbConnection, string dbScript)
        {
            await dbConnection.OpenAsync();

            await using var dbCommand = dbConnection.CreateCommand();

            dbCommand.CommandText = dbScript;

            await dbCommand.ExecuteNonQueryAsync();

            await dbConnection.CloseAsync();
        }

        private static async Task<bool> TableExistsAsync(DbConnection dbConnection, string tableName)
        {
            await dbConnection.OpenAsync();

            await using var dbCommand = dbConnection.CreateCommand();

            dbCommand.CommandText = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}'";

            var count = await dbCommand.ExecuteScalarAsync() as int?;

            await dbConnection.CloseAsync();

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