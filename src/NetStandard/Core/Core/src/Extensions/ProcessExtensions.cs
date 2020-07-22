using System;
using System.Diagnostics;
using Teronis.Diagnostics;

namespace Teronis.Extensions
{
    public static class ProcessExtensions
    {
        public static void Start(this Process process, bool echoCommand = false, string? commandEchoPrefix = null)
        {
            process = process ?? throw new ArgumentNullException(nameof(process));
            echoCommand |= commandEchoPrefix != null;

            if (echoCommand && process.StartInfo != null) {
                var commandEcho = ProcessStartInfoUtils.GetExecutionInfoText(process.StartInfo, commandEchoPrefix);
                Console.Error.WriteLine(commandEcho);
            }

            process.Start();
        }
    }
}
