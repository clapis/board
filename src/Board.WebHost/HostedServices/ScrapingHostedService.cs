using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Board.Application.Jobs.ScrapeJobs;
using Board.WebHost.Configuration;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Board.WebHost.HostedServices
{
    internal class ScrapingHostedService : IHostedService, IDisposable
    {
        private Timer _timer;

        private readonly ScrapingSettings _settings;
        private readonly IServiceProvider _services;
        private readonly ILogger<ScrapingHostedService> _logger;

        public ScrapingHostedService(
            IServiceProvider services,
            ILogger<ScrapingHostedService> logger,
            IOptions<ScrapingSettings> settings)
        {
            _logger = logger;
            _services = services;
            _settings = settings.Value;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Scraping Service init ({nameof(_settings.IsEnabled)}: {_settings.IsEnabled}, {nameof(_settings.Interval)}: {_settings.Interval})");

            if (_settings.IsEnabled)
            {
                _timer = new Timer(Execute, null, TimeSpan.Zero, _settings.Interval);
            }

            return Task.CompletedTask;
        }

        private async void Execute(object state)
        {
            _logger.LogInformation($"Scraping started.");

            try
            {
                using var scope = _services.CreateScope();

                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var sw = Stopwatch.StartNew();

                await mediator.Send(new ScrapeJobsCommand());

                sw.Stop();

                _logger.LogInformation($"Scraping completed. ({sw.ElapsedMilliseconds})ms");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Scraping failed.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Scrapping Service shutting down.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
