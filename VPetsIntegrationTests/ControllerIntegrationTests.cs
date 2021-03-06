﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VPets;
using VPets.Domain.Models;
using VPets.Resources;
using Xunit;

namespace VPetsIntegrationTests
{
    public class ControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>> 
    {
        private readonly HttpClient client;

        public ControllerIntegrationTests(CustomWebApplicationFactory<Startup> factory)
        {
            client = factory.CreateClient();
        }

        [Fact]
        public async Task CanGetPets()
        {

            // Arrange
            var request = "/api/v1/pets";

            // Act
            var httpResponse = await client.GetAsync(request);

            // Assert response
            httpResponse.EnsureSuccessStatusCode();

            // Assert content
            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<IEnumerable<PetResource>>(jsonResponse);
            Assert.True(response.ToList().Count > 1);
        }

        [Fact]
        public async Task CanGetPet()
        {
            // Arrange
            var request = "/api/v1/pets/100";

            // Act
            var httpResponse = await client.GetAsync(request);

            // Assert response
            httpResponse.EnsureSuccessStatusCode();

            // Assert content
            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<PetResource>(jsonResponse);
            Assert.Equal("Meowser", response.Name);
        }

        [Fact]
        public async Task CannotGetUnknownPet()
        {
            // Arrange
            var request = "/api/v1/pets/999";

            // Act
            var httpResponse = await client.GetAsync(request);

            // Assert response
            var response = await httpResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
        }

        [Fact]
        public async Task CannotCreatePetWithUnknownUser()
        {
            // Arrange
            var request = new
            {
                Url = "/api/v1/pets",
                Body = new
                {
                    Name = "Barksberg",
                    Type = PetType.DOG,
                    UserId = 999
                }
            };

            // Act
            var httpResponse = await client.PostAsync(request.Url,
                new StringContent(JsonConvert.SerializeObject(request.Body), Encoding.Default, "application/json"));

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }

        [Fact]
        public async Task CanCreatePet()
        {
            // Arrange
            var request = new
            {
                Url = "/api/v1/pets",
                Body = new
                {
                    Name = "Kitkat",
                    Type = PetType.CAT,
                    UserId = 1
                }
            };

            // Act
            var httpResponse = await client.PostAsync(request.Url,
                new StringContent(JsonConvert.SerializeObject(request.Body), Encoding.Default, "application/json"));
            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<CreatedPetResource>(jsonResponse);
            Assert.Equal("Kitkat", response.Name);
            Assert.Equal(0.50, response.Metrics["hunger"]);
            Assert.Equal(0.50, response.Metrics["happiness"]);
        }

        [Fact]
        public async Task CanDeletePet()
        {
            // Arrange
            var createRequest = new
            {
                Url = "/api/v1/pets",
                Body = new
                {
                    Name = "Delete Me",
                    Type = PetType.CAT,
                    UserId = 1
                }
            };

            var httpCreateResponse = await client.PostAsync(createRequest.Url,
                new StringContent(JsonConvert.SerializeObject(createRequest.Body), Encoding.Default, "application/json"));

            var jsonCreateResponse = await httpCreateResponse.Content.ReadAsStringAsync();
            var createResponse = JsonConvert.DeserializeObject<CreatedPetResource>(jsonCreateResponse);

            var deleteRequest = $"/api/v1/pets/{createResponse.Id}";

            // Act
            var httpResponse = await client.DeleteAsync(deleteRequest);

            // Assert response
            httpResponse.EnsureSuccessStatusCode();
            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<PetResource>(jsonResponse);
            Assert.Equal("Delete Me", response.Name);
        }

        [Fact]
        public async Task CanInteractPet()
        {
            // Arrange
            var getRequest = "/api/v1/pets/100";
            var interactRequest = "/api/v1/pets/1/interact?onMetric=hunger";

            var httpGetResponse = await client.GetAsync(getRequest);
            var jsonGetResponse = await httpGetResponse.Content.ReadAsStringAsync();
            var getResponse = JsonConvert.DeserializeObject<PetResource>(jsonGetResponse);

            // Act
            var httpPostResponse = await client.PostAsync(interactRequest, null);

            // Assert
            var jsonPostResponse = await httpPostResponse.Content.ReadAsStringAsync();
            var postResponse = JsonConvert.DeserializeObject<PetResource>(jsonPostResponse);

            Assert.True(getResponse.Metrics["hunger"] > postResponse.Metrics["hunger"]);
        }

        [Fact]
        public async Task CannotInteractPetWithBadMetricType()
        {
            // Arrange
            var interactRequest = "/api/v1/pets/1/interact?onMetric=AAAH";

            // Act
            var httpPostResponse = await client.PostAsync(interactRequest, null);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpPostResponse.StatusCode);
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
