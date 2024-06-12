namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration.SeedData
{
    using System;
    using System.IO;
    using System.Reflection;
    using Microsoft.EntityFrameworkCore;

    public static class SeedQuartzClustering
    {
        public static void Init(DbContext context)
        {
            var tableName = "__QRTZ_LOCKS";
            var tableExists = TableExists(context, tableName);

            if (!tableExists)
            {
                var currentDatabase = context.Database.GetDbConnection().Database;

                var script = GetEmbeddedResource("Common.Infrastructure.EntityFramework.Configuration.SeedData.QuartzClusteringSQLServerData.sql");
                script = script.Replace("[currentDatabase]", currentDatabase);

                context.Database.ExecuteSqlRaw(script);
            }
        }

        private static bool TableExists(DbContext context, string tableName)
        {
            var dbConnection = context.Database.GetDbConnection();
            dbConnection.Open();
            var dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}'";
            var count = (int)dbCommand.ExecuteScalar();
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