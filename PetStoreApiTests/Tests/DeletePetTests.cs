
using NUnit.Framework;
using PetstoreApiTests;
using RestSharp;
using System.Net;

namespace PetStoreApiTests.Tests
{
    public class DeletePetTests
    {
        private RestClient client;

        [SetUp]
        public void Setup()
        {
            var baseUrl = TestContextLoader.Configuration["baseUrl"];
            client = new RestClient(baseUrl);
        }

        [Test]
        public void DeletePet_ShouldReturn200()
        {
            var request = new RestRequest("pet/123456", Method.Delete);

            var response = client.Execute(request);
            Assert.That(HttpStatusCode.OK, Is.EqualTo(response.StatusCode));
        }

        [TearDown]
        public void TearDown()
        {
            client?.Dispose();
        }
    }
}
