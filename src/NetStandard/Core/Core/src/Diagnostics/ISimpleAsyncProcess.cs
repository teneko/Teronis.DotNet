// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;

namespace Teronis.Diagnostics
{
    public interface ISimpleAsyncProcess : IDisposable
    {
        string? CommandEchoPrefix { get; }
        bool EchoCommand { get; }
        bool HasProcessStarted { get; }

        void Start();
        Task WaitForExitAsync();
        Task<string> WaitForExitButReadOutputAsync();
        void Kill();
    }
}
