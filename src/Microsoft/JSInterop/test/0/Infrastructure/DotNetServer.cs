using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.FileSystemGlobbing;
using Teronis.Diagnostics;
using Teronis.IO;

namespace Teronis.Microsoft.JSInterop.Test.Infrastructure
{
    public class DotNetServer : IDisposable
    {
        public HttpClient HttpClient {
            get {
                EnsureNotDisposed();

                if (httpClient is null) {
                    httpClient = new HttpClient() {
                        BaseAddress = new Uri(ApplicationUrl)
                    };
                }

                return httpClient;
            }
        }

        public CancellationToken ServerStopped { get; }
        public string ApplicationUrl { get; }

        public bool HasServerStarted =>
            !(serverProcess is null);

        private CancellationTokenSource pingCancellationTokenSource;
        private HttpClient? httpClient;
        private SimpleProcess? serverProcess;
        private Task? waitUntilPingSucceededTask;
        private bool isDisposed;

        public DotNetServer(DotNetServerOptions options)
        {
            if (options is null) {
                throw new ArgumentNullException(nameof(options));
            }

            pingCancellationTokenSource = new CancellationTokenSource();
            ServerStopped = pingCancellationTokenSource.Token;
            ApplicationUrl = options.ApplicationUrl ?? "http://localhost:59595";
        }

        public DotNetServer()
            : this(new DotNetServerOptions())
        { }

        private void EnsureNotDisposed()
        {
            if (isDisposed) {
                throw new InvalidOperationException("Instance already disposed.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="OperationCanceledException">Server stopped.</exception>
        /// <exception cref="InvalidOperationException">Server already started.</exception>
        private void EnsureStartableServer()
        {
            ServerStopped.ThrowIfCancellationRequested();

            if (serverProcess is null) {
                return;
            }

            throw new InvalidOperationException("Server already started.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="OperationCanceledException">Server stopped.</exception>
        /// <exception cref="InvalidOperationException">Server already started.</exception>
        /// <exception cref="InvalidOperationException">Marker file not found.</exception>
        public void StartServer()
        {
            EnsureStartableServer();

            var markerFile = FileUtils.GetPathOfFileAbove(".test") ??
                throw new InvalidOperationException("Marker file not found.");

            var relativeProjectGlob = File.ReadAllText(markerFile.FullName).Trim();

            var absoluteProjectPath = new Matcher()
                .AddInclude(relativeProjectGlob)
                .GetResultsInFullPath(markerFile.DirectoryName)
                .First();

            var serverProcessStartInfo = new SimpleProcessStartInfo(
                name: "dotnet",
                args: $"run --project {absoluteProjectPath} --urls={ApplicationUrl}");

            serverProcess = new SimpleAsyncProcess(serverProcessStartInfo);
            serverProcess.Start();
        }

        private async Task WaitUntilPingSucceededAsyncPrototype()
        {
            HttpResponseMessage response;

            do {
                ServerStopped.ThrowIfCancellationRequested();
                response = await HttpClient.GetAsync("/", ServerStopped);
                await Task.Delay(200, ServerStopped);
            } while (response.StatusCode != HttpStatusCode.OK);
        }

        public Task WaitUntilPingSucceededAsync() =>
            waitUntilPingSucceededTask ??= WaitUntilPingSucceededAsyncPrototype();

        public void Cancel()
        {
            if (!ServerStopped.IsCancellationRequested) {
                return;
            }

            if (HasServerStarted)
                serverProcess?.Kill();

            pingCancellationTokenSource.Cancel();
        }

        public void Dispose()
        {
            if (isDisposed) {
                return;
            }

            Cancel();
            serverProcess?.Dispose();
            httpClient?.Dispose();
            pingCancellationTokenSource.Dispose();

            isDisposed = true;
        }
    }
}
