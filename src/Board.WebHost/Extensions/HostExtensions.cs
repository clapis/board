using Board.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Board.WebHost.Extensions
{
    public static class HostExtensions
    {
        public static IHost RunDbMigrations(this IHost host)
        {
            using var scope = host.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<BoardDbContext>();

            context.Database.Migrate();

            return host;
        }
    }
}
