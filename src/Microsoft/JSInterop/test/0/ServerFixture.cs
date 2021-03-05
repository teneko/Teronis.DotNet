using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Teronis.Diagnostics;
using Xunit;

namespace Teronis.Microsoft.JSInterop.Test.Server
{
    public class ServerFixture : IAsyncLifetime
    {
        public const int CancelAwaitingSuccessfulPingAfter = 5 * 1000;

        public HttpClient HttpClient { get; private set; }

        private CancellationTokenSource pingCancellationTokenSource;
        private CancellationToken pingCancellationToken;

        public ServerFixture()
        {
            //HttpClient = new HttpClient() {
            //    BaseAddress = new Uri("http://localhost:5039")
            //};

            //pingCancellationTokenSource = new CancellationTokenSource();
            //pingCancellationToken = pingCancellationTokenSource.Token;
        }

        protected void EnsureStartServer() {
            //SimpleProcess.RunAsync("dotnet", "args", wor
        }

        public Task InitializeAsync() =>
            Task.CompletedTask;

        public Task WaitForSuccessfulPingAsync()
        {
            return Task.Run(
                async () => {
                    var response = await HttpClient.GetAsync("/ping");

                    while (response.StatusCode != HttpStatusCode.OK) {
                        await Task.Delay(200);
                        continue;
                    }
                },
                pingCancellationToken);
        }

        public Task DisposeAsync()
        {
            pingCancellationTokenSource.Dispose();
            return Task.CompletedTask;
        }
    }
}
