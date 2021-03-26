// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace Teronis.Microsoft.JSInterop.Facades.Infrastructure
{
    public class TestWebHostFixture<StartupType> : IAsyncLifetime
        where StartupType : class
    {
        public IHost Host {
            get {
                EnsureStartTestHost();
                return testHost.Host;
            }
        }

        public HttpClient HttpClient {
            get {
                EnsureStartTestHost();
                return testHost.HttpClient;
            }
        }

        public string ApplicationUrl {
            get {
                EnsureStartTestHost();
                return testHost.ApplicationUrl;
            }
        }

        private TestHost testHost;
        private bool hasTestHostStarted;

        public TestWebHostFixture() =>
            testHost = new TestWebHost<StartupType>();

        private void EnsureStartTestHost()
        {
            if (hasTestHostStarted) {
                return;
            }

            testHost.StartServer();
            hasTestHostStarted = true;
        }

        public Task InitializeAsync() =>
            Task.CompletedTask;

        async Task IAsyncLifetime.DisposeAsync() =>
            await testHost.DisposeAsync();
    }
}
