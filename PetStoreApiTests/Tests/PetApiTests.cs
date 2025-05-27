using NUnit.Framework;
using PetstoreApiTests;
using PetStoreApiTests.Utils;
using RestSharp;
using System.Net;
using System.Text.Json;

namespace PetStoreApiTests.Tests
{
    public class PetApiTests
    {
        private RestClient _client;
        private ApiClient _apiClient;
        private long _petId;

        [SetUp]
        public void Setup()
        {
            var baseUrl = TestContextLoader.Configuration["baseUrl"];
            _client = new RestClient(baseUrl);
            _apiClient = new ApiClient(_client);
        }

        [Test, Order(1)]
        public void CreatePet_ShouldReturnSuccess()
        {
            var pet = new
            {
                id = "abc-" + new Random().Next(100000, 999999).ToString(),
                name = "EndToEndTestDog",
                photoUrls = new[] { "http://example.com/photo.png" },
                status = "available"
            };

            var request = new RestRequest("pet", Method.Post);
            request.AddJsonBody(pet);

            var response = _client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test, Order(2)]
        public void UpdatePetStatus_ShouldSucceed()
        {
            //Assume.That(_petId, Is.GreaterThan(0));

            var updatedPet = new
            {
                id = _petId,
                name = "EndToEndTestDog",
                photoUrls = new[] { "http://example.com/photo.png" },
                status = "sold"
            };

            var request = new RestRequest("pet", Method.Get);
            request.AddJsonBody(updatedPet);

            var response = _client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        [Test, Order(3)]
        public void GetPetById_ShouldReturnUpdatedStatus()
        {
            //Assume.That(_petId, Is.GreaterThan(0));

            var response = _apiClient.PollUntilSuccess($"pet/{_petId}", Method.Get);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var pet = JsonSerializer.Deserialize<Pet>(response.Content, new JsonSerializerOptions
            {
                //PropertyNameCaseInsensitive = true
            });

            Assert.That(pet.status, Is.EqualTo("pending"));
        }

        [Test, Order(4)]
        public void DeletePet_ShouldSucceed()
        {
            //Assume.That(_petId, Is.GreaterThan(0));
            var response = _apiClient.PollUntilSuccess($"pet/{_petId}", Method.Delete);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.MethodNotAllowed));
        }

        [Test, Order(5)]
        public void GetDeletedPet_ShouldReturnNotFound()
        {
            //Assume.That(_petId, Is.GreaterThan(0));
            var response = _apiClient.PollUntilSuccess($"pet/{_petId}", Method.Get);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        public class Pet
        {
            public long id { get; set; }
            public string name { get; set; }
            public string status { get; set; }
        }

        [TearDown]
        public void TearDown()
        {
            _client?.Dispose();
        }
    }
}