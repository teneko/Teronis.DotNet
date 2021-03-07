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
