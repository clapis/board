using System;
using System.Threading.Tasks;
using Board.Infrastructure.Database;
using Board.WebHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace Board.IntegrationTests.Api
{
    [CollectionDefinition(nameof(ApiTestFixture))]
    public class ApiTestFixtureCollection : ICollectionFixture<ApiTestFixture> { }

    public class ApiTestFixture : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Integration");

            builder.ConfigureServices(services =>
            {
                // Replace default DbContext with an in-memory context
                services.AddDbContext<TestDbContext>();
                services.AddScoped<BoardDbContext, TestDbContext>();
            });
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            var host = base.CreateHost(builder);

            SeedTestDbContext(host);

            return host;
        }

        private void SeedTestDbContext(IHost host)
        {
            using var scope = host.Services.CreateScope();

            var context = host.Services.GetRequiredService<TestDbContext>();

            context.Seed();

        }

        internal async Task<T> ExecuteDbContextAsync<T>(Func<TestDbContext, Task<T>> action)
        {
            using var scope = Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<TestDbContext>();

            return await action(context);

        }
    }

}
