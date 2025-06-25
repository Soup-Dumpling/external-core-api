using Alba;
using System.Threading.Tasks;
using Xunit;

namespace External.Product.Api.IntegrationTests.Controllers
{
    [Collection("Integration")]
    public class VersionControllerTests
    {
        private readonly IAlbaHost host;

        public VersionControllerTests(AppFixture fixture)
        {
            host = fixture.Host;
        }

        [Fact]
        public async Task GetVersion()
        {
            await host.Scenario(_ =>
            {
                _.Get.Url("/api/version");
                _.ContentShouldBe("1.0.0");
            });
        }
    }
}
