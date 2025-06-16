using Alba;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace External.Product.Api.IntegrationTests
{
    public class AppFixture : IDisposable, IAsyncLifetime
    {
        public IAlbaHost Host { get; private set; }

        public void Dispose()
        {
            Host?.Dispose();
        }
        
        public async Task InitializeAsync()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            Environment.SetEnvironmentVariable("DisableMigration", "true");

            Host = await Program.CreateHostBuilder(Array.Empty<string>()).StartAlbaAsync();
        }

        public async Task DisposeAsync()
        {
            await Host.StopAsync();
        }
    }

    [CollectionDefinition("Integration")]
    public class AppFixtureCollection : ICollectionFixture<AppFixture>
    {

    }
}
