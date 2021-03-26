// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Diagnostics
{
    public interface ISimpleProcess : IDisposable
    {
        string? CommandEchoPrefix { get; }
        bool EchoCommand { get; }
        bool HasProcessStarted { get; }

        void Start();
        void WaitForExit();
        string WaitForExitButReadOutput();
        void Kill();
    }
}
