using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Board.Application.Jobs.GetJobs;
using Xunit;

namespace Board.IntegrationTests.Api.Jobs
{
    [Collection(nameof(ApiTestFixture))]
    public class GetJobsTests
    {
        private readonly ApiTestFixture _fixture;

        public GetJobsTests(ApiTestFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task Get_ReturnsSuccess()
        {
            var client = _fixture.CreateClient();

            var response = await client.GetAsync("/jobs");

            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            var model = await JsonSerializer.DeserializeAsync<List<GetJobsQueryResult>>(stream);

            Assert.NotNull(model);
        }
    }
}
