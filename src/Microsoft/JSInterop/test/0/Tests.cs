using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Test.Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace Teronis.Microsoft.JSInterop.Test
{
    public class Tests : IClassFixture<TestWebHostFixture<WebHostStartup>>
    {
        private readonly ITestOutputHelper output;
        private readonly TestWebHostFixture<WebHostStartup> server;

        public Tests(ITestOutputHelper output, TestWebHostFixture<WebHostStartup> server)
        {
            this.output = output;
            this.server = server;
        }

        [Fact]
        public async Task Should()
        {
            var applicationUrl = server.ApplicationUrl;
            await Task.Delay(60 * 1000 * 6);
            ;
        }
    }
}
