using System;
using System.Diagnostics;

namespace Teronis.MSBuild.Tools
{
    public static class ProcessTools
    {
        public static void Run(string name, string? args = null, Action<string>? outputReceived = null, Action<string>? errorReceived = null)
        {
            ProcessStartInfo processInfo;

            if (args is null)
                processInfo = new ProcessStartInfo(name);
            else
                processInfo = new ProcessStartInfo(name, args);

            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardOutput = true;
            processInfo.RedirectStandardError = true;
            processInfo.CreateNoWindow = true;

            var process = new Process() {
                StartInfo = processInfo,
            };

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
    }
}
