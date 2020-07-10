using System;
using System.Windows.Forms;

namespace Teronis.Extensions
{
    public static class ControlExtensions
    {
        public static void InvokeIfNeeded(this Control control, Action action)
        {
            if (control.InvokeRequired) {
                control.Invoke(action);
            } else {
                action();
            }
        }

        public static void Suspend(this Control control)
        {
            var msgSuspendUpdate = Message.Create(control.Handle, Windows.WindowsDefaults.WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
            var window = NativeWindow.FromHandle(control.Handle);
            window.DefWndProc(ref msgSuspendUpdate);
        }

        public static void Resume(this Control control)
        {
            // Create a C "true" boolean as an IntPtr
            var wparam = new IntPtr(1);
            var msgResumeUpdate = Message.Create(control.Handle, Windows.WindowsDefaults.WM_SETREDRAW, wparam, IntPtr.Zero);
            var window = NativeWindow.FromHandle(control.Handle);
            window.DefWndProc(ref msgResumeUpdate);
        }
    }
}
