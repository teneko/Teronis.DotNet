// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;

namespace Teronis.Diagnostics
{
    public partial class SimpleProcess
    {
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
        public static void StartAndWaitForExit(
            SimpleProcessStartInfo startInfo,
            bool echoCommand = false,
            string? commandEchoPrefix = null,
            Action<string?>? outputReceived = null,
            Action<string?>? errorReceived = null)
        {
            using var process = new SimpleProcess(
                startInfo,
                echoCommand: echoCommand,
                commandEchoPrefix: commandEchoPrefix,
                outputReceived: outputReceived,
                errorReceived: errorReceived);

            process.Start();
            process.WaitForExit();
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
        public static async Task StartAndWaitForExitAsync(
            SimpleProcessStartInfo startInfo,
            bool echoCommand = false,
            string? commandEchoPrefix = null,
            Action<string?>? outputReceived = null,
            Action<string?>? errorReceived = null)
        {
            using var process = new SimpleAsyncProcess(
                startInfo,
                echoCommand: echoCommand,
                commandEchoPrefix: commandEchoPrefix,
                outputReceived: outputReceived,
                errorReceived: errorReceived);

            await process.WaitForExitAsync();
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
        public static string StartAndWaitForExitButReadOutput(
            SimpleProcessStartInfo startInfo,
            bool echoCommand = false,
            string? commandEchoPrefix = null,
            Action<string?>? errorReceived = null)
        {
            using var process = new SimpleProcess(
               startInfo,
               echoCommand: echoCommand,
               commandEchoPrefix: commandEchoPrefix,
               errorReceived: errorReceived);

            process.Start();
            return process.WaitForExitButReadOutput();
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
        public static async Task<string> StartAndWaitForExitButReadOutputAsync(
            SimpleProcessStartInfo startInfo,
            bool echoCommand = false,
            string? commandEchoPrefix = null,
            Action<string?>? errorReceived = null)
        {
            using var process = new SimpleAsyncProcess(
               startInfo,
               echoCommand: echoCommand,
               commandEchoPrefix: commandEchoPrefix,
               errorReceived: errorReceived);

            process.Start();
            return await process.WaitForExitButReadOutputAsync();
        }
    }
}
