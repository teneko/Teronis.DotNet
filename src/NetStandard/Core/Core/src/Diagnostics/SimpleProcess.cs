using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Teronis.Threading.Tasks;

namespace Teronis.Diagnostics
{
    public static class SimpleProcess
    {
        private static Process prepareProcess(string name, string? args, Action<string>? outputReceived, Action<string>? errorReceived)
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

            return process;
        }

        public static void Run(string name, string? args = null, Action<string>? outputReceived = null, Action<string>? errorReceived = null)
        {
            var process = prepareProcess(name, args, outputReceived, errorReceived);

            void onOutputDataReceived(object sender, DataReceivedEventArgs e) =>
                outputReceived?.Invoke(e.Data);

            process.OutputDataReceived += onOutputDataReceived;

            void onErrorDataReceived(object sender, DataReceivedEventArgs e) =>
                errorReceived?.Invoke(e.Data);

            process.ErrorDataReceived += onErrorDataReceived;

            try {
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();

                if (process.ExitCode != 0) {
                    throw new NonZeroExitCodeException(process.ExitCode);
                }
            } finally {
                process.OutputDataReceived -= onOutputDataReceived;
                process.ErrorDataReceived -= onOutputDataReceived;
            }
        }

        public static Task RunAsync(string name, string? args = null, Action<string>? outputReceived = null, Action<string>? errorReceived = null)
        {
            var process = prepareProcess(name, args, outputReceived, errorReceived);

            var processCompletionSource = new TaskCompletionSource();

            void dispose()
            {
                process.Exited -= onExited;
                process.OutputDataReceived -= onOutputDataReceived;
                process.ErrorDataReceived -= onOutputDataReceived;
            }

            void onExited(object sender, EventArgs e)
            {
                dispose();

                if (process.ExitCode == 0) {
                    processCompletionSource.SetResult();
                } else {
                    var error = new NonZeroExitCodeException(process.ExitCode);
                    processCompletionSource.SetException(error);
                }
            }

            process.Exited += onExited;

            void onOutputDataReceived(object sender, DataReceivedEventArgs e) =>
                outputReceived?.Invoke(e.Data);

            process.OutputDataReceived += onOutputDataReceived;

            void onErrorDataReceived(object sender, DataReceivedEventArgs e) =>
                errorReceived?.Invoke(e.Data);

            process.ErrorDataReceived += onErrorDataReceived;

            try {
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
            } catch (Exception error) {
                processCompletionSource.SetException(error);
            } finally {
                dispose();
            }

            return processCompletionSource.Task;
        }
    }
}
