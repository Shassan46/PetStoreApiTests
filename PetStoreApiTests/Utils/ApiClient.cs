using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PetStoreApiTests.Utils
{
    internal class ApiClient
    {
        private readonly RestClient _client; // Make it readonly

        public ApiClient(RestClient client) // Constructor to inject RestClient
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public RestResponse PollUntilSuccess(string resource, Method method, int maxAttempts = 5, int delayMs = 3000)
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
