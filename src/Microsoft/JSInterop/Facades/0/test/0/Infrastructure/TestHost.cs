// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Net.Http;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Teronis.Microsoft.JSInterop.Facades.Infrastructure
{
    public abstract class TestHost : IAsyncDisposable
    {
        /// <summary>
        /// The application url from <see cref="Host"/>.
        /// It is available after the server started.
        /// </summary>
        public string ApplicationUrl => applicationUrl ??
            throw new InvalidOperationException();

        /// <summary>
        /// The http client with <see cref="ApplicationUrl"/> 
        /// as base address. It is available after the server
        /// started.
        /// </summary>
        public HttpClient HttpClient {
            get {
                if (httpClient is null) {
                    if (applicationUrl is null) {
                        throw new InvalidOperationException();
                    }

                    httpClient = new HttpClient() {
                        BaseAddress = new Uri(ApplicationUrl)
                    };
                }

                return httpClient;
            }
        }

        public CancellationToken ServerStopped { get; }

        public IHost Host =>
            host ?? throw new InvalidOperationException();

        public bool HasServerStarted =>
            !(Host is null);

        private CancellationTokenSource pingCancellationTokenSource;
        private string? applicationUrl;
        private HttpClient? httpClient;
        private IHost? host;
        private Task? hostStartTask;
        private bool isDisposed;

        public TestHost()
        {
            pingCancellationTokenSource = new CancellationTokenSource();
            ServerStopped = pingCancellationTokenSource.Token;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="OperationCanceledException">Server stopped.</exception>
        /// <exception cref="InvalidOperationException">Server already started.</exception>
        private void EnsureStartableServer()
        {
            ServerStopped.ThrowIfCancellationRequested();

            if (host is null) {
                return;
            }

            throw new InvalidOperationException("Server already started.");
        }

        /// <summary>
        /// Creates the host.
        /// </summary>
        /// <returns></returns>
        protected abstract IHost CreateHost();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <exception cref="TimeoutException">Host could not start.</exception>
        private void StartHostInstance(IHost host)
        {
            if (host is null) {
                throw new InvalidOperationException($"Host instance is null.");
            }

            var resetEvent = new ManualResetEventSlim();
            ExceptionDispatchInfo? capturedExceptionInfo = null;

            hostStartTask = new Task(async () => {
                try {
                    await host.StartAsync(ServerStopped);
                } catch (Exception error) {
                    capturedExceptionInfo = ExceptionDispatchInfo.Capture(error);
                }

                resetEvent.Set();
            }, ServerStopped, TaskCreationOptions.LongRunning);

            hostStartTask.Start();

            if (!resetEvent.Wait(TimeSpan.FromSeconds(10), ServerStopped)) {
                throw new TimeoutException("Host could not start.");
            }

            if (!(capturedExceptionInfo is null)) {
                capturedExceptionInfo.Throw();
            }
        }

        /// <summary>
        /// Starts the server.
        /// </summary>
        /// <exception cref="OperationCanceledException">Server stopped.</exception>
        /// <exception cref="InvalidOperationException">Server already started.</exception>
        public void StartServer()
        {
            EnsureStartableServer();
            var host = CreateHost();

            StartHostInstance(host);

            applicationUrl = host.Services.GetRequiredService<IServer>().Features
                .Get<IServerAddressesFeature>()
                .Addresses.Single();

            this.host = host;
        }

        public async ValueTask CancelAsync()
        {
            if (!ServerStopped.IsCancellationRequested) {
                return;
            }

            if (HasServerStarted) {
                await host!.StopAsync();
            }

            pingCancellationTokenSource.Cancel();
        }

        public async ValueTask DisposeAsync()
        {
            if (isDisposed) {
                return;
            }

            await CancelAsync();
            hostStartTask?.Dispose();
            host?.Dispose();
            httpClient?.Dispose();
            pingCancellationTokenSource.Dispose();

            isDisposed = true;
        }
    }
}
