// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Diagnostics
{
    public class NonZeroExitCodeException : Exception
    {
        public int ExitCode { get; }

        public NonZeroExitCodeException(int exitCode) =>
            ExitCode = exitCode;

        public NonZeroExitCodeException(int exitCode, string? message)
            : base(message) =>
            ExitCode = exitCode;

        public NonZeroExitCodeException(int exitCode, string? message, Exception? innerException)
            : base(message, innerException) =>
            ExitCode = exitCode;
    }
}
