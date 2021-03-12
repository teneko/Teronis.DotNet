using System.Threading.Tasks;
using PlaywrightSharp;
using Xunit;

namespace Teronis.Microsoft.JSInterop.Facade.Infrastructure
{
    public class PlaywrightFixture : IAsyncLifetime
    {
        public IPlaywright Instance { get; private set; } = null!;

        public async Task InitializeAsync() =>
            Instance = await Playwright.CreateAsync();

        public Task DisposeAsync()
        {
            Instance.Dispose();
            return Task.CompletedTask;
        }
    }
}
