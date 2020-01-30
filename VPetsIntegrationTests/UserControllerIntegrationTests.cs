using System.Net.Http;
using System.Threading.Tasks;
using VPets;
using Xunit;

namespace VPetsIntegrationTests
{
    public class UserControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient client;

        public UserControllerIntegrationTests(CustomWebApplicationFactory<Startup> factory)
        {
            client = factory.CreateClient();
        }

        [Fact]
        public async Task CanGetUsers()
        {
            // Arrange
            var request = "/api/v1/users";

            // Act
            var httpResponse = await client.GetAsync(request);

            // Assert
            httpResponse.EnsureSuccessStatusCode();
        }
    }
}
