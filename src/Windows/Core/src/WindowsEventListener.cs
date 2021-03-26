// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Windows
{
    public class WindowEventListener : IDisposable
    {
        public delegate void WinEventDelegate(WindowEventListener listener, IntPtr hWnd);

        public event WinEventDelegate? WinEventRaised;

        readonly Win32.WinEventDelegate win32EventHandler;
        readonly IntPtr hookId;

        /// <summary>
        /// With this class you are able to listen to global window event(s).
        /// </summary>
        /// <param name="winEvent"><see cref="WINEVENT"/></param>
        /// <param name="dwFlags"><see cref="WINEVENT.WINEVENT_OUTOFCONTEXT"/> | <see cref="WINEVENT.WINEVENT_SKIPOWNPROCESS"/></param>
        public WindowEventListener(uint winEvent, uint dwFlags)
        {
            win32EventHandler = new Win32.WinEventDelegate(desktop_win32EventRaised);
            hookId = Win32.SetWinEventHook(winEvent, winEvent, IntPtr.Zero, win32EventHandler, 0, 0, dwFlags);
        }

        private void desktop_win32EventRaised(IntPtr hWinEventHook, uint eventType, IntPtr hWnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
            => WinEventRaised?.Invoke(this, hWnd);

        public void Dispose() => Win32.UnhookWinEvent(hookId);
    }
}
