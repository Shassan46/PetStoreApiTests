
using NUnit.Framework;
using PetstoreApiTests;
using RestSharp;
using System.Net;

namespace PetStoreApiTests.Tests
{
    public class GetPetTests
    {
        private RestClient client;

        [SetUp]
        public void Setup()
        {
            var baseUrl = TestContextLoader.Configuration["baseUrl"];
            client = new RestClient(baseUrl);
        }

        [Test]
        public void GetExistingPet_ShouldReturn200()
        {
            var request = new RestRequest("pet/123456", Method.Get);

            var response = client.Execute(request);
            Assert.That(HttpStatusCode.OK, Is.EqualTo(response.StatusCode));
        }

        [Test]
        public void GetNonExistingPet_ShouldReturn404()
        {
            var request = new RestRequest("pet/9999999", Method.Get);

            var response = client.Execute(request);
            Assert.That(HttpStatusCode.NotFound, Is.EqualTo(response.StatusCode));
        }
    }
}
