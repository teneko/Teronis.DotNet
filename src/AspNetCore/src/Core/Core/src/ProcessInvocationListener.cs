using System;
using System.Diagnostics;
using System.Management;
using System.Threading.Tasks;

namespace Teronis
{
    public class ProcessInvocationListener : IDisposable
    {
        public event Action<Process> ProcessCreated;

        ManagementEventWatcher mew;
        string processName;

        public ProcessInvocationListener(string processName)
        {
            this.processName = processName;
            mew = new ManagementEventWatcher(string.Format("SELECT * FROM Win32_ProcessStartTrace WITHIN 0.1 WHERE ProcessName = '{0}'", processName));
            mew.EventArrived += Event_Arrived;
        }

        private void addValidProcess(Process process) => ProcessCreated?.Invoke(process);

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
                tryAddProcess(Process.GetProcessById(int.Parse(e.NewEvent.Properties["ProcessID"].Value.ToString())));
            } catch (ArgumentException) { }
        }

        public void StartListener() => mew.Start();

        public void ListenToExistingProccesses()
        {
            foreach (var process in Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(processName)))
                tryAddProcess(process);
        }

        public void StopListener() => mew.Stop();

        public void Dispose()
        {
            mew.Stop();
            mew.Dispose();
            mew = null;
        }
    }
}
