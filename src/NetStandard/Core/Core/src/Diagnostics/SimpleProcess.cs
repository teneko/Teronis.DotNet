using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Teronis.Extensions;
using Teronis.Threading.Tasks;

namespace Teronis.Diagnostics
{
    public static class SimpleProcess
    {
        private static Process prepareProcess(
            string name,
            string? args,
            string? workingDirectory,
            string? commandEchoPrefix,
            Action<string?>? outputReceived,
            Action<string?>? errorReceived,
            out Action dettachHandlers,
            out Func<NonZeroExitCodeException> createNonZeroExitCodeException)
        {
            ProcessStartInfo processInfo;

            if (args is null) {
                processInfo = new ProcessStartInfo(name) {
                    WorkingDirectory = workingDirectory
                };
            } else {
                processInfo = new ProcessStartInfo(name, args) {
                    WorkingDirectory = workingDirectory
                };
            }

            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardOutput = true;
            processInfo.RedirectStandardError = true;
            processInfo.CreateNoWindow = true;

            var process = new Process() {
                StartInfo = processInfo,
            };

            void dettachHandlersDefinition()
            {
                process.Exited -= onExited;
                process.OutputDataReceived -= onOutputDataReceived;
                process.ErrorDataReceived -= onOutputDataReceived;
            }

            void onExited(object? sender, EventArgs e) =>
                dettachHandlersDefinition();

            process.Exited += onExited;

            void onOutputDataReceived(object? sender, DataReceivedEventArgs e) =>
                outputReceived?.Invoke(e.Data);

            if (outputReceived != null) {
                process.OutputDataReceived += onOutputDataReceived;
            }

            var errorMessageBuilder = new StringBuilder();

            void onErrorDataReceived(object? sender, DataReceivedEventArgs e)
            {
                errorMessageBuilder.Append(e.Data);
                errorReceived?.Invoke(e.Data);
            }

            process.ErrorDataReceived += onErrorDataReceived;

            NonZeroExitCodeException createNonZeroExitCodeExceptionDefinition()
            {
                var isErrorMessageBuilderEmpty = errorMessageBuilder.Length == 0;

                var executionInfoText = ProcessStartInfoUtils.GetExecutionInfoText(processInfo, commandEchoPrefix: commandEchoPrefix) +
                    (isErrorMessageBuilderEmpty ? "" : Environment.NewLine);

                errorMessageBuilder.Insert(0, executionInfoText);
                return new NonZeroExitCodeException(process.ExitCode, errorMessageBuilder.ToString());
            }

            dettachHandlers = dettachHandlersDefinition;
            createNonZeroExitCodeException = createNonZeroExitCodeExceptionDefinition;
            return process;
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
        public static void Run(
            string name,
            string? args = null,
            string? workingDirectory = null,
            bool echoCommand = false,
            string? commandEchoPrefix = null,
            Action<string?>? outputReceived = null,
            Action<string?>? errorReceived = null)
        {
            var process = prepareProcess(
                name,
                args: args,
                workingDirectory: workingDirectory,
                commandEchoPrefix: commandEchoPrefix,
                outputReceived: outputReceived,
                errorReceived: errorReceived,
                out var dispose,
                out var createNonZeroExitCodeException);

            try {
                process.Start(echoCommand: echoCommand, commandEchoPrefix: commandEchoPrefix);
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();

                if (process.ExitCode != 0) {
                    throw createNonZeroExitCodeException();
                }
            } finally {
                dispose();
            }
        }

        private static ValueTask<string> readAsyncOrNot(
            bool readAsync,
            string name,
            string? args,
            string? workingDirectory,
            bool echoCommand,
            string? commandEchoPrefix,
            Action<string?>? errorReceived)
        {
            var output = new StringBuilder();

            void onOutputReceived(string? receivedOutput) =>
                output.Append(receivedOutput);

            if (readAsync) {
                return new ValueTask<string>(
                        RunAsync(
                            name,
                            args: args,
                            workingDirectory: workingDirectory,
                            echoCommand: echoCommand,
                            commandEchoPrefix: commandEchoPrefix,
                            outputReceived: onOutputReceived,
                            errorReceived: errorReceived)
                    .ContinueWith(task => output.ToString()));
            } else {
                Run(name,
                    args: args,
                    workingDirectory: workingDirectory,
                    echoCommand: echoCommand,
                    commandEchoPrefix: commandEchoPrefix,
                    outputReceived: onOutputReceived,
                    errorReceived: errorReceived);

                return new ValueTask<string>(output.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <param name="workingDirectory">The working directory is not used to find the executable. Instead, its value applies to the process that is started and only has meaning within the context of the new process.</param>
        /// <param name="echoCommand"></param>
        /// <param name="commandEchoPrefix"></param>
        /// <param name="errorReceived"></param>
        /// <returns></returns>
        public static string Read(
            string name,
            string? args = null,
            string? workingDirectory = null,
            bool echoCommand = false,
            string? commandEchoPrefix = null,
            Action<string?>? errorReceived = null) =>
            /// We can grab for <see cref="ValueTask{string}.Result"/> safely.
            readAsyncOrNot(
                readAsync: false,
                name,
                args: args,
workingDirectory: workingDirectory,
                echoCommand: echoCommand,
                commandEchoPrefix: commandEchoPrefix,
                errorReceived: errorReceived).Result;

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
        public static Task RunAsync(
            string name,
            string? args = null,
            string? workingDirectory = null,
            bool echoCommand = false,
            string? commandEchoPrefix = null,
            Action<string?>? outputReceived = null,
            Action<string?>? errorReceived = null)
        {
            var process = prepareProcess(
                name,
                args: args,
                workingDirectory: workingDirectory,
                commandEchoPrefix: commandEchoPrefix,
                outputReceived: outputReceived,
                errorReceived: errorReceived,
                out Action innerDispose,
                out var createNonZeroExitCodeException);

            var processCompletionSource = new TaskCompletionSource();

            void dispose()
            {
                innerDispose();
                process.Exited -= onExited;
            }

            void onExited(object? sender, EventArgs e)
            {
                dispose();

                if (process.ExitCode == 0) {
                    processCompletionSource.SetResult();
                } else {
                    var error = createNonZeroExitCodeException();
                    processCompletionSource.SetException(error);
                }
            }

            process.EnableRaisingEvents = true;
            process.Exited += onExited;

            try {
                process.Start(echoCommand: echoCommand, commandEchoPrefix: commandEchoPrefix);
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
            } catch (Exception error) {
                processCompletionSource.SetException(error);
                dispose();
            }

            return processCompletionSource.Task;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <param name="workingDirectory">The working directory is not used to find the executable. Instead, its value applies to the process that is started and only has meaning within the context of the new process.</param>
        /// <param name="echoCommand"></param>
        /// <param name="commandEchoPrefix"></param>
        /// <param name="errorReceived"></param>
        /// <returns></returns>
        public static ValueTask<string> ReadAsync(
            string name,
            string? args = null,
            string? workingDirectory = null,
            bool echoCommand = false,
            string? commandEchoPrefix = null,
            Action<string?>? errorReceived = null) =>
           readAsyncOrNot(
               readAsync: true,
               name,
               args: args,
               workingDirectory: workingDirectory,
               echoCommand: echoCommand,
               commandEchoPrefix: commandEchoPrefix,
               errorReceived: errorReceived);
    }
}
