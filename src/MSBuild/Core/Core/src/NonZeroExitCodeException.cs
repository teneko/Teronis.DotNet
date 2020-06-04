using System;

namespace Teronis.MSBuild
{
    public class NonZeroExitCodeException : Exception
    {
        public int ExitCode { get; }

        public NonZeroExitCodeException(int exitCode)
        {
            ExitCode = exitCode;
        }

        public NonZeroExitCodeException(int exitCode, string message)
            : base(message)
        {
            ExitCode = exitCode;
        }

        public NonZeroExitCodeException(int exitCode, string message, Exception innerException)
            : base(message, innerException)
        {
            ExitCode = exitCode;
        }
    }
}
