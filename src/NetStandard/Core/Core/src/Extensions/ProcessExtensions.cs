// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using Teronis.Diagnostics;

namespace Teronis.Extensions
{
    public static class ProcessExtensions
    {
        /// <summary>
        /// true if a process resource is started; false if no new process resource is started
        /// </summary>
        /// <param name="process"></param>
        /// <param name="echoCommand"></param>
        /// <param name="commandEchoPrefix"></param>
        /// <returns></returns>
        public static bool Start(this Process process, bool echoCommand = false, string? commandEchoPrefix = null)
        {
            process = process ?? throw new ArgumentNullException(nameof(process));
            echoCommand |= commandEchoPrefix != null;

            if (echoCommand && process.StartInfo != null) {
                var commandEcho = ProcessStartInfoUtils.GetExecutionInfoText(process.StartInfo, commandEchoPrefix);
                Console.Error.WriteLine(commandEcho);
            }

            return process.Start();
        }
    }
}
