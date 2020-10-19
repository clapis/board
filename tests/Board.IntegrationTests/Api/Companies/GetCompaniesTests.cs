using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Board.Application.Companies.GetCompanies;
using Xunit;

namespace Board.IntegrationTests.Api.Companies
{
    [Collection(nameof(ApiTestFixture))]
    public class GetCompaniesTests
    {
        private readonly ApiTestFixture _fixture;

        public GetCompaniesTests(ApiTestFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task Get_ReturnsSuccess()
        {
            // Arrange 
            var client = _fixture.CreateClient();

            // Act
            var response = await client.GetAsync("/companies");

            // Assert
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            var model = await JsonSerializer.DeserializeAsync<List<GetCompaniesQueryResult.Company>>(stream);

            Assert.NotNull(model);
        }

        
    }
}
