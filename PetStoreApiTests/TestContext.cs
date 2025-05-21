using Microsoft.Extensions.Configuration;

namespace PetstoreApiTests
{
    public static class TestContextLoader
    {
        public static IConfigurationRoot Configuration { get; }

        static TestContextLoader()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }
    }
}
