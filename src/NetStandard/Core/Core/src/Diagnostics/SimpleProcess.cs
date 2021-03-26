// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Text;
using Teronis.Extensions;

namespace Teronis.Diagnostics
{
    partial class SimpleProcess : IDisposable, ISimpleProcess
    {
        public bool EchoCommand { get; }
        public string? CommandEchoPrefix { get; }
        public bool HasProcessStarted { get; private set; }

        private Action<string?>? outputReceived { get; }
        private Action<string?>? errorReceived { get; }

        private readonly ProcessStartInfo processStartInfo;
        private Process? process;
        private StringBuilder? outputDataBuilder;
        private StringBuilder errorMessageBuilder;
        private bool isDisposed;

        public SimpleProcess(
            SimpleProcessStartInfo processStartInfo,
            bool echoCommand = false,
            string? commandEchoPrefix = null,
            Action<string?>? outputReceived = null,
            Action<string?>? errorReceived = null)
        {
            errorMessageBuilder = new StringBuilder();

            if (processStartInfo is null) {
                throw new ArgumentNullException(nameof(processStartInfo));
            }

            this.processStartInfo = processStartInfo.ProcessStartInfo;
            EchoCommand = echoCommand;
            CommandEchoPrefix = commandEchoPrefix;
            this.outputReceived = outputReceived;
            this.errorReceived = errorReceived;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="InvalidOperationException">Process already started.</exception>
        protected Process GetCreatedProcess()
        {
            if (!(process is null)) {
                return process;
            }

            throw new InvalidOperationException("Process has not been started.");
        }

        protected string GetOutputData()
        {
            if (outputDataBuilder is null) {
                throw new InvalidOperationException("Reading output data is not allowed.");
            }

            return outputDataBuilder.ToString();
        }

        private void DettachProcessHandlers()
        {
            var process = GetCreatedProcess();
            process.Exited -= OnProcessExited;
            process.OutputDataReceived -= OnOutputDataReceived;
            process.ErrorDataReceived -= OnOutputDataReceived;
        }

        protected virtual void OnProcessExited(object? sender, EventArgs e) =>
                DettachProcessHandlers();

        private void OnOutputDataReceived(object? sender, DataReceivedEventArgs e)
        {
            outputDataBuilder?.Append(e.Data);
            outputReceived?.Invoke(e.Data);
        }

        private void OnErrorDataReceived(object? sender, DataReceivedEventArgs e)
        {
            errorMessageBuilder.Append(e.Data);
            errorReceived?.Invoke(e.Data);
        }

        /// <summary>
        /// You may create it before the process starts.
        /// </summary>
        protected void CreateOutputDataBuilder()
        {
            if (!(outputDataBuilder is null)) {
                throw new InvalidOperationException("Output data builder alreay created.");
            }

            outputDataBuilder = new StringBuilder();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="InvalidOperationException">Process alreay created.</exception>
        protected virtual Process PrepareProcess()
        {
            if (!(process is null)) {
                throw new InvalidOperationException("Process alreay created.");
            }

            process = new Process() { StartInfo = processStartInfo };
            process.EnableRaisingEvents = true;
            process.Exited += OnProcessExited;
            process.OutputDataReceived += OnOutputDataReceived;
            process.ErrorDataReceived += OnErrorDataReceived;
            return process;
        }

        protected NonZeroExitCodeException CreateNonZeroExitCodeException()
        {
            var process = GetCreatedProcess();
            var isErrorMessageBuilderEmpty = errorMessageBuilder.Length == 0;

            var executionInfoText = ProcessStartInfoUtils.GetExecutionInfoText(processStartInfo, commandEchoPrefix: CommandEchoPrefix) +
                (isErrorMessageBuilderEmpty ? "" : Environment.NewLine);

            errorMessageBuilder.Insert(0, executionInfoText);
            return new NonZeroExitCodeException(process.ExitCode, errorMessageBuilder.ToString());
        }

        /// <summary>
        /// When an error occurred during process start.
        /// </summary>
        protected virtual void OnProcessNotStarted(Exception error) { }

        private Exception CreateProcessNotStartedException() =>
            new ProcessNotSpawnedException();

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="InvalidOperationException">Process already created.</exception>
        public void Start()
        {
            var process = PrepareProcess();
            bool processSpawned;

            try {
                processSpawned = process.Start(echoCommand: EchoCommand, commandEchoPrefix: CommandEchoPrefix);
            } catch (Exception error) {
                OnProcessNotStarted(error);
                throw;
            }

            if (!processSpawned) {
                OnProcessNotStarted(CreateProcessNotStartedException());
            }

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            HasProcessStarted = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="InvalidOperationException">Process not started yet.</exception>
        protected void EnsureProcessStarted()
        {
            if (HasProcessStarted) {
                return;
            }

            throw new InvalidOperationException("Process not started yet.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NonZeroExitCodeException"></exception>
        protected void ThrowIfNonZeroExitCode()
        {
            var process = GetCreatedProcess();

            if (process.ExitCode != 0) {
                throw CreateNonZeroExitCodeException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NonZeroExitCodeException"></exception>
        public void WaitForExit()
        {
            EnsureProcessStarted();
            var process = GetCreatedProcess();
            process.WaitForExit();
            ThrowIfNonZeroExitCode();
        }

        public string WaitForExitButReadOutput()
        {
            EnsureProcessStarted();
            CreateOutputDataBuilder();
            WaitForExit();
            return GetOutputData();
        }

        public void Kill()
        {
            EnsureProcessStarted();

            GetCreatedProcess()
                .Kill();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) {
                return;
            }

            if (disposing) {
                var process = GetCreatedProcess();
                DettachProcessHandlers();
                process.Dispose();
            }

            isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
