
using NUnit.Framework;
using PetstoreApiTests;
using RestSharp;
using System.Net;

namespace PetStoreApiTests.Tests
{
    public class AddPetTests
    {
        private RestClient client;

        [SetUp]
        public void Setup()
        {
            var baseUrl = TestContextLoader.Configuration["baseUrl"];
            client = new RestClient(baseUrl);
        }

        [Test]
        public void AddPet_ShouldReturn200()
        {
            ;
            var request = new RestRequest("pet", Method.Post);
            request.AddJsonBody(new
            {
                id = 123456,
                name = "doggie",
                status = "available"
            });

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
