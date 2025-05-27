
using NUnit.Framework;
using PetstoreApiTests;
using PetStoreApiTests.Utils;
using RestSharp;
using System.Net;

namespace PetStoreApiTests.Tests
{
    public class GetPetTests
    {
        private RestClient client;
        private ApiClient _apiClient;

        [SetUp]
        public void Setup()
        {
            var baseUrl = TestContextLoader.Configuration["baseUrl"];
            client = new RestClient(baseUrl);
            _apiClient = new ApiClient(client); // Instantiate ApiClient here
        }

        [Test]
        public void GetExistingPet_ShouldReturn200()
        {
            int _petId = 123456; // Replace with a valid pet ID
            var response = _apiClient.PollUntilSuccess($"pet/{_petId}", Method.Get);
            Assert.That(HttpStatusCode.OK, Is.EqualTo(response.StatusCode));
        }

        [Test]
        public void GetNonExistingPet_ShouldReturn404()
        {
            var request = new RestRequest("pet/98989898", Method.Get);

            var response = client.Execute(request);
            Assert.That(HttpStatusCode.NotFound, Is.EqualTo(response.StatusCode));
        }

        [TearDown]
        public void TearDown()
        {
            client?.Dispose();
        }
    }
}
