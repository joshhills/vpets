using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VPets;
using VPets.Resources;
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

            // Assert response
            httpResponse.EnsureSuccessStatusCode();

            // Assert content
            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<IEnumerable<UserResource>>(jsonResponse);
            Assert.True(response.ToList().Count > 1);
        }

        [Fact]
        public async Task CanGetUser()
        {
            // Arrange
            var request = "/api/v1/users/1";

            // Act
            var httpResponse = await client.GetAsync(request);

            // Assert response
            httpResponse.EnsureSuccessStatusCode();

            // Assert content
            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<UserResource>(jsonResponse);
            Assert.Equal("Josh", response.Name);
        }

        [Fact]
        public async Task CanGetPetsForUser()
        {
            // Arrange
            var request = "/api/v1/users/1/pets";

            // Act
            var httpResponse = await client.GetAsync(request);

            // Assert response
            httpResponse.EnsureSuccessStatusCode();

            // Assert content
            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<IEnumerable<UserPetResource>>(jsonResponse);
            Assert.True(response.ToList().Count > 1);
        }

        [Fact]
        public async Task CannotGetUnknownUser()
        {
            // Arrange
            var request = "/api/v1/users/999";

            // Act
            var httpResponse = await client.GetAsync(request);

            // Assert response
            var response = await httpResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
        }

        [Fact]
        public async Task CannotCreateUserWithBadRequest()
        {
            // Arrange
            var request = new
            {
                Url = "/api/v1/users",
                Body = new
                {
                    Name = "-!@'/a:X.-!@'/a:X.-!@'/a:X.-!@'/a:X."
                }
            };

            // Act
            var httpResponse = await client.PostAsync(request.Url,
                new StringContent(JsonConvert.SerializeObject(request.Body), Encoding.Default, "application/json"));

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }

        [Fact]
        public async Task CanCreateUser()
        {
            // Arrange
            var request = new
            {
                Url = "/api/v1/users",
                Body = new
                {
                    Name = "Johnny"
                }
            };

            // Act
            var httpResponse = await client.PostAsync(request.Url,
                new StringContent(JsonConvert.SerializeObject(request.Body), Encoding.Default, "application/json"));
            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<UserResource>(jsonResponse);
            Assert.Equal("Johnny", response.Name);
        }

        [Fact]
        public async Task CanDeleteUser()
        {
            // Arrange
            var createRequest = new
            {
                Url = "/api/v1/users",
                Body = new
                {
                    Name = "Delete Me"
                }
            };

            var httpCreateResponse = await client.PostAsync(createRequest.Url,
                new StringContent(JsonConvert.SerializeObject(createRequest.Body), Encoding.Default, "application/json"));

            var jsonCreateResponse = await httpCreateResponse.Content.ReadAsStringAsync();
            var createResponse = JsonConvert.DeserializeObject<UserResource>(jsonCreateResponse);

            var deleteRequest = $"/api/v1/users/{createResponse.Id}";

            // Act
            var httpResponse = await client.DeleteAsync(deleteRequest);

            // Assert response
            httpResponse.EnsureSuccessStatusCode();
            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<UserResource>(jsonResponse);
            Assert.Equal("Delete Me", response.Name);
        }
    }
}
