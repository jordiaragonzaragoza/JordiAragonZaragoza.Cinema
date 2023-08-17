namespace JordiAragon.Cinema.Presentation.WebApi.FunctionalTests.Common
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.DependencyInjection;
    using Testcontainers.SqlEdge;
    using Xunit;

    public class FunctionalTestsFixture<TProgram> : IAsyncLifetime, IDisposable
        where TProgram : class
    {
        private readonly SqlEdgeContainer container = new SqlEdgeBuilder().WithImage("mcr.microsoft.com/azure-sql-edge:latest").WithAutoRemove(true).Build();
        private SqlConnection connection;
        private bool disposedValue;

        ////private readonly IServiceScopeFactory scopeFactory;

        public CustomWebApplicationFactory<TProgram> CustomApplicationFactory { get; private set; }

        public async Task InitializeAsync()
        {
            await this.container.StartAsync();

            this.connection = new SqlConnection(this.container.GetConnectionString());

            this.CustomApplicationFactory = new CustomWebApplicationFactory<TProgram>(this.connection);

            ////this.scopeFactory = this.customApplicationFactory.Services.GetRequiredService<IServiceScopeFactory>();
        }

        public async Task DisposeAsync()
        {
            await this.connection.DisposeAsync();
            await this.container.DisposeAsync();
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.CustomApplicationFactory.Dispose();
                }

                this.CustomApplicationFactory = null;
                this.disposedValue = true;
            }
        }
    }
}