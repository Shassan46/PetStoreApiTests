
using NUnit.Framework;
using PetstoreApiTests;
using RestSharp;
using System.Net;

namespace PetStoreApiTests.Tests
{
    public class UpdatePetTests
    {
        private RestClient client;

        [SetUp]
        public void Setup()
        {
            var baseUrl = TestContextLoader.Configuration["baseUrl"];
            client = new RestClient(baseUrl);
        }

        [TearDown]
        public void TearDown()
        {
            client?.Dispose();
        }

        [Test]
        public void UpdatePet_ShouldReturn200()
        {
            var request = new RestRequest("pet", Method.Put);
            request.AddJsonBody(new
            {
                id = 123456,
                name = "doggie-updated",
                status = "sold"
            });

            var response = client.Execute(request);
            Assert.That(HttpStatusCode.OK, Is.EqualTo(response.StatusCode));
        }
    }
}
