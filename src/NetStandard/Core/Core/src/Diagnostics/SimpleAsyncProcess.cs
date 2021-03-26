// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Teronis.Threading.Tasks;

namespace Teronis.Diagnostics
{
    public class SimpleAsyncProcess : SimpleProcess, ISimpleAsyncProcess
    {
        private TaskCompletionSource processCompletionSource = null!;
        private bool isDisposed;

        public SimpleAsyncProcess(
            SimpleProcessStartInfo processStartInfo,
            bool echoCommand = false,
            string? commandEchoPrefix = null,
            Action<string?>? outputReceived = null,
            Action<string?>? errorReceived = null)
            : base(
                  processStartInfo,
                  echoCommand,
                  commandEchoPrefix,
                  outputReceived,
                  errorReceived)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="InvalidOperationException">Processc completion source already created.</exception>
        private void CreateProcessCompletionSource()
        {
            if (!(processCompletionSource is null)) {
                throw new InvalidOperationException();
            }

            processCompletionSource = new TaskCompletionSource();
        }

        protected override void OnProcessExited(object? sender, EventArgs e)
        {
            var process = GetCreatedProcess();

            if (process.ExitCode == 0) {
                processCompletionSource.SetResult();
            } else {
                var error = CreateNonZeroExitCodeException();
                processCompletionSource.SetException(error);
            }

            base.OnProcessExited(sender, e);
        }

        protected override Process PrepareProcess()
        {
            CreateProcessCompletionSource();
            return base.PrepareProcess();
        }

        protected override void OnProcessNotStarted(Exception error)
        {
            processCompletionSource.SetException(error);
            base.OnProcessNotStarted(error);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <param name="workingDirectory">The working directory is not used to find the executable. Instead, its value applies to the process that is started and only has meaning within the context of the new process.</param>
        /// <param name="echoCommand"></param>
        /// <param name="commandEchoPrefix"></param>
        /// <param name="outputReceived"></param>
        /// <param name="errorReceived"></param>
        /// <returns></returns>
        public Task WaitForExitAsync()
        {
            EnsureProcessStarted();
            return processCompletionSource.Task;
        }

        public async Task<string> WaitForExitButReadOutputAsync()
        {
            CreateOutputDataBuilder();
            await WaitForExitAsync();
            return GetOutputData();
        }

        /// <summary>
        /// Releases all resources used by <see cref="SimpleAsyncProcess"/>.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (isDisposed) {
                return;
            }

            if (disposing) {
                processCompletionSource.SetCanceled();
            }

            isDisposed = true;
        }
    }
}
