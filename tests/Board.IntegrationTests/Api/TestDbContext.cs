using System.Collections.Generic;
using Board.Core.Abstractions;
using Board.Core.Entities;
using Board.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Board.IntegrationTests.Api
{
    public class TestDbContext : BoardDbContext
    {
        public TestDbContext(DbContextOptions<BoardDbContext> options,
            IOptions<DatabaseSettings> settings, IDomainEventDispatcher dispatcher)
            : base(options, settings, dispatcher)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase(nameof(TestDbContext));
        }

        public void Seed()
        {
            var company = new Company("Integration Tests Inc.");

            var jobs = new List<Job> {
                new Job("Junior QA Engineer", company, "https://qa.jobs/junior-qa-engineer/"),
                new Job("Medior QA Engineer", company, "https://qa.jobs/medior-qa-engineer/"),
                new Job("Senior QA Engineer", company, "https://qa.jobs/senior-qa-engineer/")
            };

            jobs.ForEach(job => job.Publish(PublicationType.Free));

            AddRange(jobs);
            SaveChanges();
        }

    }
}
