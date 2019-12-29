using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Teronis.Windows
{
    public static class Win32
    {
        #region ClientToScreen

        [DllImport("user32.dll", EntryPoint = "ClientToScreen")]
        private static extern bool clientToScreen(IntPtr hWnd, ref Point lpPoint);

        public static Point ClientToScreen(IntPtr hWnd, Point point)
        {
            if (!clientToScreen(hWnd, ref point))
                throw new Exception(Constants.ErrorMessageWhileImportingDll);

            return point;
        }

        #endregion

        #region ScreenToClient

        [DllImport("user32.dll", EntryPoint = "ScreenToClient")]
        private static extern bool screenToClient(IntPtr hWnd, ref Point lpPoint);

        public static Point ScreenToClient(IntPtr hWnd, Point point)
        {
            if (!screenToClient(hWnd, ref point))
                throw new Exception(Constants.ErrorMessageWhileImportingDll);

            return point;
        }

        #endregion
    }
}
