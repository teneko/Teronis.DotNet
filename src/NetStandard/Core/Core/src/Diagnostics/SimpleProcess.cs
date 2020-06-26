using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Teronis.Threading.Tasks;
using Teronis.Extensions;

namespace Teronis.Diagnostics
{
    public static class SimpleProcess
    {
        private static Process prepareProcess(string name, string? args, string? commandEchoPrefix, Action<string>? outputReceived, Action<string>? errorReceived, out Action dettachHandlers, out Func<NonZeroExitCodeException> createNonZeroExitCodeException)
        {
            ProcessStartInfo processInfo;

            if (args is null) {
                processInfo = new ProcessStartInfo(name);
            } else {
                processInfo = new ProcessStartInfo(name, args);
            }

            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardOutput = true;
            processInfo.RedirectStandardError = true;
            processInfo.CreateNoWindow = true;

            var process = new Process() {
                StartInfo = processInfo,
            };

            void _dettachHandlers()
            {
                process.Exited -= onExited;
                process.OutputDataReceived -= onOutputDataReceived;
                process.ErrorDataReceived -= onOutputDataReceived;
            }

            void onExited(object sender, EventArgs e) =>
                _dettachHandlers();

            process.Exited += onExited;

            void onOutputDataReceived(object sender, DataReceivedEventArgs e) =>
                outputReceived?.Invoke(e.Data);

            if (outputReceived != null) {
                process.OutputDataReceived += onOutputDataReceived;
            }

            var errorMessageBuilder = new StringBuilder();

            void onErrorDataReceived(object sender, DataReceivedEventArgs e)
            {
                errorMessageBuilder.Append(e.Data);
                errorReceived?.Invoke(e.Data);
            }

            process.ErrorDataReceived += onErrorDataReceived;

            NonZeroExitCodeException _createNonZeroExitCodeException()
            {
                var isErrorMessageBuilderEmpty = errorMessageBuilder.Length == 0;

                var executionInfoText = ProcessStartInfoTools.GetExecutionInfoText(processInfo, commandEchoPrefix: commandEchoPrefix) +
                    (isErrorMessageBuilderEmpty ? "" : Environment.NewLine);

                errorMessageBuilder.Insert(0, executionInfoText);
                return new NonZeroExitCodeException(process.ExitCode, errorMessageBuilder.ToString());
            }

            dettachHandlers = _dettachHandlers;
            createNonZeroExitCodeException = _createNonZeroExitCodeException;
            return process;
        }

        public static void Run(string name, string? args = null, bool echoCommand = false, string? commandEchoPrefix = null, Action<string>? outputReceived = null, Action<string>? errorReceived = null)
        {
            var process = prepareProcess(name, args, commandEchoPrefix, outputReceived, errorReceived, out var dispose, out var createNonZeroExitCodeException);

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

        private static ValueTask<string> readAsyncOrNot(bool readAsync, string name, string? args, bool echoCommand, string? commandEchoPrefix, Action<string>? errorReceived)
        {
            var output = new StringBuilder();

            void onOutputReceived(string receivedOutput)
            {
                output.Append(receivedOutput);
            }

            if (readAsync) {
                return new ValueTask<string>(RunAsync(name, args, echoCommand, commandEchoPrefix, onOutputReceived, errorReceived)
                    .ContinueWith(task => output.ToString()));
            } else {
                Run(name, args, echoCommand, commandEchoPrefix, onOutputReceived, errorReceived);
                return new ValueTask<string>(output.ToString());
            }
        }

        public static string Read(string name, string? args = null, bool echoCommand = false, string? commandEchoPrefix = null, Action<string>? errorReceived = null) =>
            /// We can grab for <see cref="ValueTask{string}.Result"/> safely.
            readAsyncOrNot(false, name, args, echoCommand, commandEchoPrefix, errorReceived).Result;

        public static Task RunAsync(string name, string? args = null, bool echoCommand = false, string? commandEchoPrefix = null, Action<string>? outputReceived = null, Action<string>? errorReceived = null)
        {
            var process = prepareProcess(name, args, commandEchoPrefix, outputReceived, errorReceived, out Action innerDispose, out var createNonZeroExitCodeException);

            var processCompletionSource = new TaskCompletionSource();

            void dispose()
            {
                innerDispose();
                process.Exited -= onExited;
            }

            void onExited(object sender, EventArgs e)
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

        public static ValueTask<string> ReadAsync(string name, string? args = null, bool echoCommand = false, string? commandEchoPrefix = null, Action<string>? errorReceived = null) =>
           readAsyncOrNot(true, name, args, echoCommand, commandEchoPrefix, errorReceived);
    }
}
