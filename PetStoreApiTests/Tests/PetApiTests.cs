using NUnit.Framework;
using PetstoreApiTests;
using RestSharp;
using System.Net;
using System.Text.Json;

namespace PetStoreApiTests.Tests
{
    public class PetApiTests
    {

        private RestClient _client;
        private long _petId;

        [SetUp]
        public void Setup()
        {
            var baseUrl = TestContextLoader.Configuration["baseUrl"];
            _client = new RestClient(baseUrl);
        }


        [Test, Order(1)]
        public void CreatePet_ShouldReturnSuccess()
        {
            var pet = new
            {
                id = new Random().Next(100000, 999999),
                name = "EndToEndTestDog",
                photoUrls = new[] { "http://example.com/photo.png" },
                status = "available"
            };
            _petId = pet.id;

            var request = new RestRequest("pet", Method.Post);
            request.AddJsonBody(pet);

            var response = _client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test, Order(2)]
        public void UpdatePetStatus_ShouldSucceed()
        {
            Assume.That(_petId, Is.GreaterThan(0));

            var updatedPet = new
            {
                id = _petId,
                name = "EndToEndTestDog",
                photoUrls = new[] { "http://example.com/photo.png" },
                status = "sold"
            };

            var request = new RestRequest("pet", Method.Put);
            request.AddJsonBody(updatedPet);

            var response = _client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test, Order(3)]
        public void GetPetById_ShouldReturnUpdatedStatus()
        {
            Assume.That(_petId, Is.GreaterThan(0));

            var response = PollUntilSuccess($"pet/{_petId}", Method.Get);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var pet = JsonSerializer.Deserialize<Pet>(response.Content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.That(pet.status, Is.EqualTo("sold"));
        }

        [Test, Order(4)]
        public void DeletePet_ShouldSucceed()
        {
            Assume.That(_petId, Is.GreaterThan(0));
            var response = PollUntilSuccess($"pet/{_petId}", Method.Delete);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test, Order(5)]
        public void GetDeletedPet_ShouldReturnNotFound()
        {
            Assume.That(_petId, Is.GreaterThan(0));
            var response = PollUntilSuccess($"pet/{_petId}", Method.Get);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        public class Pet
        {
            public long id { get; set; }
            public string name { get; set; }
            public string status { get; set; }
        }

        private RestResponse PollUntilSuccess(string resource,Method method, int maxAttempts = 5, int delayMs = 3000)
        {
            RestResponse response = null;

            for (int i = 0; i < maxAttempts; i++)
            {
                var request = new RestRequest(resource, method);
                response = _client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                    break;

                Thread.Sleep(delayMs);
            }

            return response;
        }

    }
}
