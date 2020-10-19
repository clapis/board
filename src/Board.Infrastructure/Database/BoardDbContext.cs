using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Board.Core.Abstractions;
using Board.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Board.Infrastructure.Database
{
    public class BoardDbContext : DbContext
    {

        public DbSet<Job> Jobs { get; set; }

        public DbSet<Scrape> Scrapes { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<ReceivedPayment> ReceivedPayments { get; set; }


        private readonly DatabaseSettings _settings;
        private readonly IDomainEventDispatcher _dispatcher;

        public BoardDbContext(
            DbContextOptions<BoardDbContext> options,
            IOptions<DatabaseSettings> settings,
            IDomainEventDispatcher dispatcher) : base(options)
        {
            _dispatcher = dispatcher;
            _settings = settings.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
            options.UseSqlite(_settings.ConnectionString);

            base.OnConfiguring(options);
        }

        protected override void OnModelCreating(ModelBuilder model)
        {
            base.OnModelCreating(model);

            model.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return SaveChangesAsync(acceptAllChangesOnSuccess).GetAwaiter().GetResult();
        }


        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            SetCreatedOnTimestampOnAddedBaseEntities();

            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            await DispatchDomainEventsAsync();

            return result;
        }

        private void SetCreatedOnTimestampOnAddedBaseEntities()
        {
            var entries = ChangeTracker.Entries<BaseEntity>()
                    .Where(e => e.State == EntityState.Added);

            foreach (var entry in entries)
                entry.Entity.CreatedOn = DateTime.UtcNow;
        }

        private async Task DispatchDomainEventsAsync()
        {
            var entries = ChangeTracker.Entries<BaseEntity>()
                                        .Select(e => e.Entity)
                                        .Where(e => e.Events.Any());

            foreach(var entry in entries)
            {
                var events = entry.Events.ToList();

                entry.Events.Clear();

                foreach (var @event in events)
                    await _dispatcher.PublishAsync(@event);
            }
            
        }

    }
}
