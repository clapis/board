using Board.Application;
using Board.Infrastructure;
using Board.Infrastructure.Database;
using Board.WebHost.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Board.WebHost
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<PaymentSettings>(Configuration.GetSection("Board:Payments"));
            services.Configure<DatabaseSettings>(Configuration.GetSection("Board:Database"));
            services.Configure<ScrapingSettings>(Configuration.GetSection("Board:Scraping"));

            services
                .AddBoardWebHost()
                .AddBoardApplication()
                .AddBoardInfrastructure();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(opts => opts.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

            app.UseRouting();
            app.UseCors();
            app.UseAuthorization();
            app.UseResponseCaching();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
