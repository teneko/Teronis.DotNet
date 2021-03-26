// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Management;
using System.Threading.Tasks;

namespace Teronis.Managment
{
#if NET5_0
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
#endif
    public class ProcessInvocationListener : IDisposable
    {
        public event Action<Process>? ProcessCreated;

        ManagementEventWatcher mew;
        readonly string processName;

        public ProcessInvocationListener(string processName)
        {
            this.processName = processName;
            mew = new ManagementEventWatcher(string.Format("SELECT * FROM Win32_ProcessStartTrace WITHIN 0.1 WHERE ProcessName = '{0}'", processName));
            mew.EventArrived += Event_Arrived;
        }

        private void ensureNotDisposed()
        {
            if (mew == null) {
                throw new ObjectDisposedException("This instance has been already disposed.");
            }
        }

        private void addValidProcess(Process process) =>
            ProcessCreated?.Invoke(process);

        private async void tryAddProcess(Process process)
        {
            await Task.Run(() => {
                while (!process.HasExited) {
                    if (process.MainWindowHandle != IntPtr.Zero) {
                        addValidProcess(process);
                        break;
                    }
                }
            });
        }

        private void Event_Arrived(object sender, EventArrivedEventArgs e)
        {
            try {
                tryAddProcess(Process.GetProcessById(int.Parse(e.NewEvent.Properties["ProcessID"].Value.ToString()!)));
            } catch (ArgumentException) { }
        }

        public void StartListener()
        {
            ensureNotDisposed();
            mew.Start();
        }

        public void ListenToExistingProccesses()
        {
            foreach (var process in Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(processName))) {
                tryAddProcess(process);
            }
        }

        public void StopListener()
        {
            ensureNotDisposed();
            mew.Stop();
        }

        public void Dispose()
        {
            mew.Stop();
            mew.Dispose();
            mew = null!;
        }
    }
}
