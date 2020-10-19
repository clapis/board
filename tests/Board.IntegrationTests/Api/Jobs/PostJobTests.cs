using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Board.Application.Jobs.PostJob;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Board.IntegrationTests.Api.Jobs
{
    [Collection(nameof(ApiTestFixture))]
    public class PostJobTests
    {
        private readonly ApiTestFixture _fixture;
        private readonly ITestOutputHelper _logger;

        public PostJobTests(ApiTestFixture fixture, ITestOutputHelper logger)
        {
            _logger = logger;
            _fixture = fixture;
        }


        [Fact]
        public async Task Post_ValidRequest_ReturnsSuccess()
        {
            // Arrange
            var client = _fixture.CreateClient();

            var command = GetValidPostJobCommand();

            var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/jobs", content);

            _logger.WriteLine(await response.Content.ReadAsStringAsync());

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);            
        }


        [Fact]
        public async Task Post_WhenLogoIsNotValidImage_ReturnsBadRequest()
        {
            // Arrange
            var client = _fixture.CreateClient();

            var command = GetValidPostJobCommand();

            command.Logo = Encoding.UTF8.GetBytes("not_a_base64_encoded_png");

            var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/jobs", content);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Post_WithTags_SuccessfullyPersistsTags()
        {
            // Arrange
            var client = _fixture.CreateClient();

            var command = GetValidPostJobCommand();

            command.Tags = new List<string> { "it", "qa" };

            var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/jobs", content);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

            var id = await JsonSerializer.DeserializeAsync<int>(await response.Content.ReadAsStreamAsync());

            var job = await _fixture.ExecuteDbContextAsync(db => db.Jobs.SingleOrDefaultAsync(job => job.Id == id));

            Assert.NotNull(job);
            Assert.True(job.Tags.SetEquals(command.Tags));
        }


        private PostJobCommand GetValidPostJobCommand() => new PostJobCommand
        {
            Position = "QA Engineer",
            CompanyName = "Testing Inc.",
            Description = "QA Engineer Job Description",
            Remote = true,
            Website = "https://www.testing.inc",
            ApplyUrl = "https://www.testing.inc/vacancies/qa-engineer",
            Email = "admin@testing.inc",
        };
    }
}
