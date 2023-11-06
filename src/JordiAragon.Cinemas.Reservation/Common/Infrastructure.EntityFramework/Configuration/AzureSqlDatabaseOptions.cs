namespace JordiAragon.Cinemas.Reservation.Common.Infrastructure.EntityFramework.Configuration
{
    using System.Text;
    using Microsoft.Extensions.Primitives;

    public class AzureSqlDatabaseOptions
    {
        public const string Section = "AzureSqlDatabase";

        public string ApplicationName { get; set; }

        public string Server { get; set; }

        public string Database { get; set; }

        public string UserId { get; set; }

        public string Password { get; set; }

        public bool TrustedConnection { get; set; }

        public bool Encrypt { get; set; }

        public bool MultipleActiveResultSets { get; set; }

        public string BuildConnectionString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append($"Application Name={this.ApplicationName};");
            stringBuilder.Append($"Server={this.Server};");
            stringBuilder.Append($"Database={this.Database};");
            stringBuilder.Append($"user id={this.UserId};");
            stringBuilder.Append($"password={this.Password};");

            if (this.TrustedConnection)
            {
                stringBuilder.Append("Trusted_Connection=True;");
            }
            else
            {
                stringBuilder.Append("Trusted_Connection=False;");
            }

            if (this.Encrypt)
            {
                stringBuilder.Append("Encrypt=True;");
            }
            else
            {
                stringBuilder.Append("Encrypt=False;");
            }

            if (this.MultipleActiveResultSets)
            {
                stringBuilder.Append("MultipleActiveResultSets=True");
            }
            else
            {
                stringBuilder.Append("MultipleActiveResultSets=False");
            }

            return stringBuilder.ToString();
        }
    }
}