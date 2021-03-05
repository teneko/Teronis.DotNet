using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace Teronis.Windows
{
    public static class Win32
    {
        #region ClientToScreen

        [DllImport("user32.dll", EntryPoint = "ClientToScreen")]
        private static extern bool clientToScreen(IntPtr hWnd, ref Point lpPoint);

        public static Point ClientToScreen(IntPtr hWnd, Point point)
        {
            if (!clientToScreen(hWnd, ref point)) {
                throw new Exception(WindowsDefaults.ErrorMessageWhileImportingDll);
            }

            return point;
        }

        #endregion

        #region ScreenToClient

        [DllImport("user32.dll", EntryPoint = "ScreenToClient")]
        private static extern bool screenToClient(IntPtr hWnd, ref Point lpPoint);

        public static Point ScreenToClient(IntPtr hWnd, Point point)
        {
            if (!screenToClient(hWnd, ref point)) {
                throw new Exception(WindowsDefaults.ErrorMessageWhileImportingDll);
            }

            return point;
        }

        #endregion

        #region Attach | Free>Console

        [DllImport("kernel32.dll", EntryPoint = "AttachConsole")]
        public static extern bool AttachConsole(int dwProcessId);
        public const int ATTACH_PARENT_PROCESS = -1;

        [DllImport("kernel32.dll", EntryPoint = "FreeConsole", SetLastError = true)]
        public static extern int FreeConsole();

        #endregion

        #region GetCursorPos

        [DllImport("user32.dll", EntryPoint = "GetWindowThreadProcessId")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        [DllImport("user32.dll", EntryPoint = "GetThreadId")]
        public static extern int GetThreadId(IntPtr hWnd);

        /// <summary>
        /// Struct representing a point.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct WinPoint
        {
            public int X;
            public int Y;

            public static implicit operator Point(WinPoint point)
            {
                return new Point(point.X, point.Y);
            }
        }

        /// <summary>
        /// Retrieves the cursor's position, in screen coordinates.
        /// </summary>
        /// <see>See MSDN documentation for further information.</see>
        [DllImport("user32.dll", EntryPoint = "GetCursorPos")]
        private static extern bool getCursorPos(out WinPoint lpPoint);

        public static Point GetCursorPoint()
        {
            if (!getCursorPos(out WinPoint lpPoint)) {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            return lpPoint;
        }

        #endregion

        #region GetWindowLongPtr
        public const long WS_MAXIMIZE = 0x01000000L;

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern int getWindowLongPtr32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        private static extern int getWindowLongPtr64(IntPtr hWnd, int nIndex);

        // This static method is required because Win32 does not support
        // GetWindowLongPtr directly
        public static int GetWindowLong(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 8) {
                return getWindowLongPtr64(hWnd, nIndex);
            } else {
                return getWindowLongPtr32(hWnd, nIndex);
            }
        }
        #endregion

        #region SetWindowLongPtr
        public const int GWL_STYLE = -16;
        public static uint MF_BYPOSITION = 0x400;
        public static uint MF_REMOVE = 0x1000;
        public static uint WS_CHILD = 0x40000000; //child window
        public static int WS_BORDER = 0x00800000; //window with border
        public static int WS_DLGFRAME = 0x00400000; //window with double border but no title
        public static int WS_CAPTION = WS_BORDER | WS_DLGFRAME; //window with a title bar 
        public static uint WS_SYSMENU = 0x00080000; //window menu  
        public static uint WS_POPUP = 0x80000000;
        public static int WS_THICKFRAME = 0x00040000;
        public static uint WS_VSCROLL = 0x00200000;

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int setWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern int setWindowLongPtr64(IntPtr hWnd, int nIndex, int dwNewLong); // IntPtr dwNewLong

        // This static method is required because Win32 does not support
        // SetWindowLongPtr directly
        public static int SetWindowLong(IntPtr hWnd, int gwl_nIndex, int ws_dwNewLong)
        {
            if (IntPtr.Size == 8) {
                return setWindowLongPtr64(hWnd, gwl_nIndex, ws_dwNewLong);
            } else {
                return setWindowLong32(hWnd, gwl_nIndex, ws_dwNewLong);
            }
        }
        #endregion

        /// <summary>
        /// WinAPI.Mouse_Event((int)flag, mPos.X, mPos.Y, 0, 0);
        /// </summary>
        /// <param name="dwFlags"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="dwData"></param>
        /// <param name="dwExtraInfo"></param>
        [DllImport("User32.dll", EntryPoint = "mouse_event", CallingConvention = CallingConvention.Winapi)]
        public static extern void Mouse_Event(EMouseFlags dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        [DllImport("user32.dll", EntryPoint = "PostMessageA", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hwnd, uint Msg, int wParam, int lParam);

        [DllImport("User32.dll", EntryPoint = "GetSystemMetrics", CallingConvention = CallingConvention.Winapi)]
        public static extern int GetSystemMetrics(int value);

        [DllImport("user32.dll", EntryPoint = "GetForegroundWindow")]
        public static extern IntPtr GetForegroundWindow();

        #region SetWindowPos
        /// <summary>
        /// Places the window at the bottom of the Z order. If the hWnd parameter identifies a topmost window, the window loses its topmost status and is placed at the bottom of all other windows.
        /// </summary>
        public static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        /// <summary>
        /// Places the window above all non-topmost windows (that is, behind all topmost windows). This flag has no effect if the window is already a non-topmost window.
        /// </summary>
        public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        /// <summary>
        /// Places the window at the top of the Z order.
        /// </summary>
        public static readonly IntPtr HWND_TOP = new IntPtr(0);
        /// <summary>
        /// Places the window above all non-topmost windows. The window maintains its topmost position even when it is deactivated.
        /// </summary>
        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        public const UInt32 SWP_NOSIZE = 0x0001;
        public const UInt32 SWP_NOMOVE = 0x0002;
        public const UInt32 SWP_NOACTIVATE = 0x0010;

        /// <summary>
        /// Changes the size, position, and Z order of a child, pop-up, or top-level window. These windows are ordered according to their appearance on the screen. The topmost window receives the highest rank and is the first window in the Z order.
        /// </summary>
        /// <param name="hWnd">A handle to the window.</param>
        /// <param name="hWndInsertAfter">A handle to the window to precede the positioned window in the Z order. This parameter must be a window handle or one of the following values. (See HWND_...)</param>
        /// <param name="X">not used: 0</param>
        /// <param name="Y">not used: 0</param>
        /// <param name="cx">width; not used: 0</param>
        /// <param name="cy">height; not used: 0</param>
        /// <param name="uFlags">The window sizing and positioning flags. This parameter can be a combination of the following values. (See SWP_...))</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        #endregion

        #region SetCursorPos
        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        public static extern bool SetCursorPos(int X, int Y);

        public static bool SetCursorPos(Point location)
        {
            return SetCursorPos(location.X, location.Y);
        }
        #endregion

        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern int SetForegroundWindow(IntPtr hWnd);

        #region GetInnerWindowRectangle | GetOuterWindowRectangle

        [DllImport("user32.dll", EntryPoint = "GetWindowRect")]
        private static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);

        public static Rectangle GetOuterWindowRectangle(IntPtr hWnd)
        {
            // need focus? yes, otherwise: loop
            //SetForegroundWindow(hWnd);

            //var rectangle = Rectangle.Empty;
            var rect = new RECT();
            IntPtr error = GetWindowRect(hWnd, ref rect);

            int counter = 0;
            // sometimes it gives error.
            while (error == (IntPtr)0) {
                counter++;
                if (counter == 10) {
                    break;
                }

                error = GetWindowRect(hWnd, ref rect);
            }

            return rect.GetRectangle();
        }

        [DllImport("user32.dll", EntryPoint = "GetClientRect")]
        private static extern IntPtr getClientRect(IntPtr hWnd, ref RECT rect);

        public static Rectangle GetInnerWindowRectangle(IntPtr hWnd)
        {
            // need focus? yes, otherwise: loop
            //SetForegroundWindow(hWnd);

            //var rectangle = Rectangle.Empty;
            var rect = new RECT();
            IntPtr error = getClientRect(hWnd, ref rect);

            int counter = 0;
            // sometimes it gives error.
            while (error == (IntPtr)0) {
                counter++;
                if (counter == 10) {
                    break;
                }

                error = getClientRect(hWnd, ref rect);
            }

            Win32.MapWindowPoints(hWnd, GetDesktopWindow(), ref rect, 2);

            return rect.GetRectangle();
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="hdcBlt"></param>
        /// <param name="nFlags"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, uint nFlags);

        #region ShowWindow
        /// <summary>
        /// Sets the specified window's show state.
        /// </summary>
        /// <param name="hWnd">A handle to the window.</param>
        /// <param name="nCmdShow">Controls how the window is to be shown. This parameter is ignored the first time an application calls ShowWindow, if the program that launched the application provides a STARTUPINFO structure. Otherwise, the first time ShowWindow is called, the value should be the value obtained by the WinMain function in its nCmdShow parameter. In subsequent calls, this parameter can be one of the following values. (See SW_...)</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "ShowWindow")]
        public static extern IntPtr ShowWindow(IntPtr hWnd, ShowWindowCommands nCmdShow);
        #endregion

        #region DoubleBuffering - not used | Screen Capture
        /// <summary>
        /// Summary description for Win32Support.
        /// Win32Support is a wrapper class that imports all the necessary
        /// functions that are used in old double-buffering technique
        /// for smooth animation.
        /// Credits: http://www.codeguru.com/csharp/csharp/cs_graphics/drawing/article.php/c6137/Flicker-Free-Drawing-In-C.htm
        /// </summary>

        /// <summary>
        /// Enumeration to be used for those Win32 function that return BOOL
        /// </summary>
        public enum Bool
        {
            False = 0,
            True
        };

        /// <summary>
        /// CreateCompatibleDC
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        /// <summary>
        /// DeleteDC
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool DeleteDC(IntPtr hdc);

        /// <summary>
        /// ReleaseDC
        /// </summary>
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool ReleaseDC([In] IntPtr hWnd, [In] IntPtr hdc);

        /// <summary>
        /// SelectObject
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr SelectObject(IntPtr hDC,
                                                                IntPtr hObject);
        /// <summary>
        /// DeleteObject
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool DeleteObject(IntPtr hObject);
        /// <summary>
        /// CreateCompatibleBitmap
        /// </summary>
        [DllImport("gdi32.dll",
                      ExactSpelling = true,
                      SetLastError = true)]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hObject,
                                                                            int width,
                                                                            int height);

        /// <summary>
        ///    Performs a bit-block transfer of the color data corresponding to a
        ///    rectangle of pixels from the specified source device context into
        ///    a destination device context.
        /// </summary>
        /// <param name="hdc">Handle to the destination device context.</param>
        /// <param name="nXDest">The leftmost x-coordinate of the destination rectangle (in pixels).</param>
        /// <param name="nYDest">The topmost y-coordinate of the destination rectangle (in pixels).</param>
        /// <param name="nWidth">The width of the source and destination rectangles (in pixels).</param>
        /// <param name="nHeight">The height of the source and the destination rectangles (in pixels).</param>
        /// <param name="hdcSrc">Handle to the source device context.</param>
        /// <param name="nXSrc">The leftmost x-coordinate of the source rectangle (in pixels).</param>
        /// <param name="nYSrc">The topmost y-coordinate of the source rectangle (in pixels).</param>
        /// <param name="dwRop">A raster-operation code.</param>
        /// <returns>
        ///    <c>true</c> if the operation succeedes, <c>false</c> otherwise. To get extended error information, call <see cref="System.Runtime.InteropServices.Marshal.GetLastWin32Error"/>.
        /// </returns>
        [DllImport("gdi32.dll", EntryPoint = "BitBlt", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BitBlt([In] IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, [In] IntPtr hdcSrc, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);

        [DllImport("user32.dll", EntryPoint = "GetWindowDC", SetLastError = true)]
        public static extern IntPtr GetWindowDC([In] IntPtr hdc);

        [DllImport("gdi32.dll", EntryPoint = "GetDIBits")]
        public unsafe static extern int GetDIBits([In] IntPtr hdc, [In] IntPtr hbitmap, uint uStartScan, uint cScanLines, [Out] byte* lpvBits, [In, Out] BITMAPINFO* bmi, DIB_Color_Mode uUsage);
        #endregion

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        /// <summary>
        /// Examines the Z order of the child windows associated with the specified parent window and retrieves a handle to the child window at the top of the Z order.
        /// </summary>
        /// <param name="IntPt">(optional) A handle to the parent window whose child windows are to be examined. If this parameter is NULL, the function returns a handle to the window at the top of the Z order.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetTopWindow([In] IntPtr hWnd);

        #region GetNextWindow
        public static uint GW_HWNDNEXT = 2;
        public static uint GW_HWNDPREV = 3;

        /// <summary>
        /// Retrieves a handle to the next or previous window in the Z-Order. The next window is below the specified window; the previous window is above.
        /// 
        ///If the specified window is a topmost window, the function searches for a topmost window.If the specified window is a top-level window, the function searches for a top-level window.If the specified window is a child window, the function searches for a child window.
        /// </summary>
        /// <param name="hWnd">A handle to a window. The window handle retrieved is relative to this window, based on the value of the wCmd parameter.</param>
        /// <param name="GW_wCmd">Indicates whether the function returns a handle to the next window or the previous window. This parameter can be either of the following values.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindow([In] IntPtr hWnd, [In] uint GW_wCmd);
        #endregion

        public enum ScrollInfoMask : uint
        {
            SIF_RANGE = 0x1,
            SIF_PAGE = 0x2,
            SIF_POS = 0x4,
            SIF_DISABLENOSCROLL = 0x8,
            SIF_TRACKPOS = 0x10,
            SIF_ALL = (SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS),
        }

        [DllImport("user32.dll")]
        public static extern bool GetScrollInfo(IntPtr hwnd, int fnBar, ref SCROLLINFO ScrollInfo);

        [DllImport("user32.dll")]
        public static extern int SetScrollInfo(IntPtr hwnd, int fnBar, [In] ref SCROLLINFO lpsi, bool fRedraw);

        [StructLayout(LayoutKind.Sequential)]
        public struct SCROLLINFO
        {
            public int cbSize;
            public int fMask;
            public int nMin;
            public int nMax;
            public int nPage;
            public int nPos;
            public int nTrackPos;
        }

        public enum ScrollBarDirection
        {
            SB_HORZ = 0,
            SB_VERT = 1,
            SB_CTL = 2,
            SB_BOTH = 3
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowScrollBar(IntPtr hWnd, int wBar, bool bShow);

        #region SetLayeredWindowAttributes
        public const int GWL_EXSTYLE = -20;
        public const int WS_EX_LAYERED = 0x80000;
        public const int LWA_ALPHA = 0x2;
        public const int LWA_COLORKEY = 0x1;
        public const int WS_EX_TRANSPARENT = 0x00000020;

        [DllImport("user32.dll")]
        public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);
        #endregion

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        #region VirtualMemory
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint dwSize, uint lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, uint lpNumberOfBytesWritten);

        #region OpenProcess
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        public static IntPtr OpenProcess(EVirtualMemoryProtection dwDesiredAccess, bool bInheritHandle, int dwProcessId)
        {
            return OpenProcess((uint)dwDesiredAccess, bInheritHandle, dwProcessId);
        }
        #endregion

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll")]
        public static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);
        #endregion

        /// <summary>
        ///     Changes the text of the specified window's title bar (if it has one). If the specified window is a control, the
        ///     text of the control is changed. However, SetWindowText cannot change the text of a control in another application.
        ///     <para>
        ///     Go to https://msdn.microsoft.com/en-us/library/windows/desktop/ms633546%28v=vs.85%29.aspx for more
        ///     information
        ///     </para>
        /// </summary>
        /// <param name="hwnd">C++ ( hWnd [in]. Type: HWND )<br />A handle to the window or control whose text is to be changed.</param>
        /// <param name="lpString">C++ ( lpString [in, optional]. Type: LPCTSTR )<br />The new title or control text.</param>
        /// <returns>
        ///     If the function succeeds, the return value is nonzero. If the function fails, the return value is zero.<br />
        ///     To get extended error information, call GetLastError.
        /// </returns>
        /// <remarks>
        ///     If the target window is owned by the current process, <see cref="SetWindowText" /> causes a WM_SETTEXT message to
        ///     be sent to the specified window or control. If the control is a list box control created with the WS_CAPTION style,
        ///     however, <see cref="SetWindowText" /> sets the text for the control, not for the list box entries.<br />To set the
        ///     text of a control in another process, send the WM_SETTEXT message directly instead of calling
        ///     <see cref="SetWindowText" />. The <see cref="SetWindowText" /> function does not expand tab characters (ASCII code
        ///     0x09). Tab characters are displayed as vertical bar(|) characters.<br />For an example go to
        ///     <see cref="!:https://msdn.microsoft.com/en-us/library/windows/desktop/ms644928%28v=vs.85%29.aspx#sending">
        ///     Sending a
        ///     Message.
        ///     </see>
        /// </remarks>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetWindowText(IntPtr hwnd, String lpString);

        #region DragLikeWindowTitleBar
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        #endregion

        /// <summary>
        /// Retrieves a handle to the desktop window. The desktop window covers the entire screen. The desktop window is the area on top of which other windows are painted.
        /// </summary>
        /// <returns>The return value is a handle to the desktop window.</returns>
        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetDesktopWindow();

        /// <summary>
        /// The GetDeviceCaps function retrieves device-specific information for the specified device.
        /// </summary>
        /// <returns></returns>
        [DllImport("gdi32.dll", EntryPoint = "GetDeviceCaps", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetDeviceCaps([In] IntPtr hdc, [In] DeviceCap nIndex);

        #region tasbar icon menu
        [DllImport("user32.dll")]
        public static extern IntPtr GetMenu(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        public static extern int GetMenuItemCount(IntPtr hMenu);

        [DllImport("user32.dll")]
        public static extern bool DrawMenuBar(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);
        #endregion

        public const int WM_PAINT = 0x000F;
        public const int WM_NCPAINT = 0x0085F;

        [DllImport("User32.dll")]
        public static extern Int64 SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        /// <summary>
        /// Retrieves the show state and the restored, minimized, and maximized positions of the specified window.
        /// </summary>
        /// <param name="hWnd">
        /// A handle to the window.
        /// </param>
        /// <param name="lpwndpl">
        /// A pointer to the WINDOWPLACEMENT structure that receives the show state and position information.
        /// <para>
        /// Before calling GetWindowPlacement, set the length member to sizeof(WINDOWPLACEMENT). GetWindowPlacement fails if lpwndpl-> length is not set correctly.
        /// </para>
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        /// <para>
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// </para>
        /// </returns>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        /// <summary>
        /// Sets the show state and the restored, minimized, and maximized positions of the specified window.
        /// </summary>
        /// <param name="hWnd">
        /// A handle to the window.
        /// </param>
        /// <param name="lpwndpl">
        /// A pointer to a WINDOWPLACEMENT structure that specifies the new show state and window positions.
        /// <para>
        /// Before calling SetWindowPlacement, set the length member of the WINDOWPLACEMENT structure to sizeof(WINDOWPLACEMENT). SetWindowPlacement fails if the length member is not set correctly.
        /// </para>
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        /// <para>
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// </para>
        /// </returns>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32", ExactSpelling = true, SetLastError = true)]
        internal static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, [In, Out] ref RECT rect, [MarshalAs(UnmanagedType.U4)] int cPoints);

        [DllImport("user32", ExactSpelling = true, SetLastError = true)]
        internal static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, [In, Out] ref Point pt, [MarshalAs(UnmanagedType.U4)] int cPoints);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, ShowWindowCommands cmdShow);

        /// <summary>
        /// SHORT state = GetKeyState(VK_INSERT);
        /// bool down = state smaller 0; 
        /// bool toggle = (state & 1) != 0;
        /// </summary>
        /// <param name="virtualKeyCode"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(UInt16 virtualKeyCode);

        [DllImport("user32.dll")]
        public static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);

        public delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        public static IntPtr GetChildWindow(IEnumerable<IntPtr> childs, Func<IntPtr, StringBuilder, int, int> callback, string caption)
        {
            var stringBuilder = new StringBuilder();

            if (childs != null) {
                foreach (var child in childs) {
                    if (child != IntPtr.Zero && callback(child, stringBuilder, caption.Length + 1) != 0 && stringBuilder.ToString().Contains(caption)) {
                        return child;
                    }
                }
            }

            return IntPtr.Zero;
        }

        public static IntPtr GetChildWindow(IntPtr parent, Func<IntPtr, StringBuilder, int, int> callback, string caption) => GetChildWindow(new WindowHandleInfo(parent).GetAllChildHandles(), callback, caption);

        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmEnableComposition(CompositionAction uCompositionAction);

        [DllImport("dwmapi.dll")]
        private static extern int DwmIsCompositionEnabled(out bool enabled);

        public static bool IsAeroEnabled() =>
            Environment.OSVersion.Version.Major >= 6
                && DwmIsCompositionEnabled(out bool enabled) == 0 && enabled;

        public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hWnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hModWinEventProc,
            WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        public static extern bool UnhookWinEvent(IntPtr hWinEventHook);
    }

    public class WindowHandleInfo
    {
        private readonly IntPtr _MainHandle;

        public WindowHandleInfo(IntPtr handle)
        {
            _MainHandle = handle;
        }

        public List<IntPtr> GetAllChildHandles()
        {
            List<IntPtr> childHandles = new List<IntPtr>();

            GCHandle gcChildhandlesList = GCHandle.Alloc(childHandles);
            IntPtr pointerChildHandlesList = GCHandle.ToIntPtr(gcChildhandlesList);

            try {
                var childProc = new Win32.EnumWindowProc(EnumWindow);
                Win32.EnumChildWindows(this._MainHandle, childProc, pointerChildHandlesList);
            } finally {
                gcChildhandlesList.Free();
            }

            return childHandles;
        }

        private bool EnumWindow(IntPtr hWnd, IntPtr lParam)
        {
            GCHandle gcChildhandlesList = GCHandle.FromIntPtr(lParam);

            if (gcChildhandlesList == default || gcChildhandlesList.Target == null) {
                return false;
            }

            List<IntPtr> childHandles = (List<IntPtr>)gcChildhandlesList.Target;
            childHandles.Add(hWnd);
            return true;
        }
    }

    /// <summary>
    /// Enumeration for the raster operations used in BitBlt.
    /// In C++ these are actually #define. But to use these
    /// constants with C#, a new enumeration type is defined.
    /// </summary>
    public enum TernaryRasterOperations : uint
    {
        SRCCOPY = 0x00CC0020,
        SRCPAINT = 0x00EE0086,
        SRCAND = 0x008800C6,
        SRCINVERT = 0x00660046,
        SRCERASE = 0x00440328,
        NOTSRCCOPY = 0x00330008,
        NOTSRCERASE = 0x001100A6,
        MERGECOPY = 0x00C000CA,
        MERGEPAINT = 0x00BB0226,
        PATCOPY = 0x00F00021,
        PATPAINT = 0x00FB0A09,
        PATINVERT = 0x005A0049,
        DSTINVERT = 0x00550009,
        BLACKNESS = 0x00000042,
        WHITENESS = 0x00FF0062,
    };

    [Flags]
    public enum EProcessAccessFlags : uint
    {
        All = 2035711,
        Terminate = 1,
        CreateThread = 2,
        VMOperation = 8,
        VMRead = 16,
        VMWrite = 32,
        DupHandle = 64,
        SetInformation = 512,
        QueryInformation = 1024,
        Synchronize = 1048576,
    }

    public enum EVirtualMemoryProtection : uint
    {
        PAGE_NOACCESS = 1,
        PAGE_READONLY = 2,
        PAGE_READWRITE = 4,
        PAGE_WRITECOPY = 8,
        PAGE_EXECUTE = 16,
        PAGE_EXECUTE_READ = 32,
        PAGE_EXECUTE_READWRITE = 64,
        PAGE_EXECUTE_WRITECOPY = 128,
        PAGE_GUARD = 256,
        PAGE_NOCACHE = 512,
        PROCESS_ALL_ACCESS = 2035711,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RGBQUAD
    {
        public byte rgbBlue;
        public byte rgbGreen;
        public byte rgbRed;
        public byte rgbReserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BITMAPINFO
    {
        public BITMAPINFOHEADER bmiHeader;
        public RGBQUAD bmiColors;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BITMAPINFOHEADER
    {
        public uint biSize;
        public int biWidth;
        public int biHeight;
        public ushort biPlanes;
        public ushort biBitCount;
        public BitmapCompressionMode biCompression;
        public uint biSizeImage;
        public int biXPelsPerMeter;
        public int biYPelsPerMeter;
        public uint biClrUsed;
        public uint biClrImportant;
    }

    public enum DeviceCap
    {
        /// <summary>
        /// Device driver version
        /// </summary>
        DRIVERVERSION = 0,
        /// <summary>
        /// Device classification
        /// </summary>
        TECHNOLOGY = 2,
        /// <summary>
        /// Horizontal size in millimeters
        /// </summary>
        HORZSIZE = 4,
        /// <summary>
        /// Vertical size in millimeters
        /// </summary>
        VERTSIZE = 6,
        /// <summary>
        /// Horizontal width in pixels
        /// </summary>
        HORZRES = 8,
        /// <summary>
        /// Vertical height in pixels
        /// </summary>
        VERTRES = 10,
        /// <summary>
        /// Number of bits per pixel
        /// </summary>
        BITSPIXEL = 12,
        /// <summary>
        /// Number of planes
        /// </summary>
        PLANES = 14,
        /// <summary>
        /// Number of brushes the device has
        /// </summary>
        NUMBRUSHES = 16,
        /// <summary>
        /// Number of pens the device has
        /// </summary>
        NUMPENS = 18,
        /// <summary>
        /// Number of markers the device has
        /// </summary>
        NUMMARKERS = 20,
        /// <summary>
        /// Number of fonts the device has
        /// </summary>
        NUMFONTS = 22,
        /// <summary>
        /// Number of colors the device supports
        /// </summary>
        NUMCOLORS = 24,
        /// <summary>
        /// Size required for device descriptor
        /// </summary>
        PDEVICESIZE = 26,
        /// <summary>
        /// Curve capabilities
        /// </summary>
        CURVECAPS = 28,
        /// <summary>
        /// Line capabilities
        /// </summary>
        LINECAPS = 30,
        /// <summary>
        /// Polygonal capabilities
        /// </summary>
        POLYGONALCAPS = 32,
        /// <summary>
        /// Text capabilities
        /// </summary>
        TEXTCAPS = 34,
        /// <summary>
        /// Clipping capabilities
        /// </summary>
        CLIPCAPS = 36,
        /// <summary>
        /// Bitblt capabilities
        /// </summary>
        RASTERCAPS = 38,
        /// <summary>
        /// Length of the X leg
        /// </summary>
        ASPECTX = 40,
        /// <summary>
        /// Length of the Y leg
        /// </summary>
        ASPECTY = 42,
        /// <summary>
        /// Length of the hypotenuse
        /// </summary>
        ASPECTXY = 44,
        /// <summary>
        /// Shading and Blending caps
        /// </summary>
        SHADEBLENDCAPS = 45,

        /// <summary>
        /// Logical pixels inch in X
        /// </summary>
        LOGPIXELSX = 88,
        /// <summary>
        /// Logical pixels inch in Y
        /// </summary>
        LOGPIXELSY = 90,

        /// <summary>
        /// Number of entries in physical palette
        /// </summary>
        SIZEPALETTE = 104,
        /// <summary>
        /// Number of reserved entries in palette
        /// </summary>
        NUMRESERVED = 106,
        /// <summary>
        /// Actual color resolution
        /// </summary>
        COLORRES = 108,

        // Printing related DeviceCaps. These replace the appropriate Escapes
        /// <summary>
        /// Physical Width in device units
        /// </summary>
        PHYSICALWIDTH = 110,
        /// <summary>
        /// Physical Height in device units
        /// </summary>
        PHYSICALHEIGHT = 111,
        /// <summary>
        /// Physical Printable Area x margin
        /// </summary>
        PHYSICALOFFSETX = 112,
        /// <summary>
        /// Physical Printable Area y margin
        /// </summary>
        PHYSICALOFFSETY = 113,
        /// <summary>
        /// Scaling factor x
        /// </summary>
        SCALINGFACTORX = 114,
        /// <summary>
        /// Scaling factor y
        /// </summary>
        SCALINGFACTORY = 115,

        /// <summary>
        /// Current vertical refresh rate of the display device (for displays only) in Hz
        /// </summary>
        VREFRESH = 116,
        /// <summary>
        /// Vertical height of entire desktop in pixels
        /// </summary>
        DESKTOPVERTRES = 117,
        /// <summary>
        /// Horizontal width of entire desktop in pixels
        /// </summary>
        DESKTOPHORZRES = 118,
        /// <summary>
        /// Preferred blt alignment
        /// </summary>
        BLTALIGNMENT = 119
    }

    public enum BitmapCompressionMode : uint
    {
        BI_RGB = 0x0000,
        BI_RLE8 = 0x0001,
        BI_RLE4 = 0x0002,
        BI_BITFIELDS = 0x0003,
        BI_JPEG = 0x0004,
        BI_PNG = 0x0005,
        BI_CMYK = 0x000B,
        BI_CMYKRLE8 = 0x000C,
        BI_CMYKRLE4 = 0x000D
    }

    /// <summary>
    /// The format of the bmiColors member of the BITMAPINFO structure. It must be one of the following values.
    /// </summary>
    public enum DIB_Color_Mode : uint
    {
        /// <summary>
        /// The color table should consist of an array of 16-bit indexes into the current logical palette.
        /// </summary>
        DIB_RGB_COLORS = 0,
        /// <summary>
        /// The color table should consist of literal red, green, blue (RGB) values.
        /// </summary>
        DIB_PAL_COLORS = 1
    }

    /// <summary>
    /// Contains information about the placement of a window on the screen.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWPLACEMENT
    {
        public static WINDOWPLACEMENT Empty = new WINDOWPLACEMENT();

        public WINDOWPLACEMENT(WINDOWPLACEMENT placement)
        {
            Length = placement.Length;
            Flags = placement.Flags;
            ShowCmd = placement.ShowCmd;
            MinPosition = placement.MinPosition;
            MaxPosition = placement.MaxPosition;
            NormalPosition = placement.NormalPosition;
        }

        /// <summary>
        /// The length of the structure, in bytes. Before calling the GetWindowPlacement or SetWindowPlacement functions, set this member to sizeof(WINDOWPLACEMENT).
        /// <para>
        /// GetWindowPlacement and SetWindowPlacement fail if this member is not set correctly.
        /// </para>
        /// </summary>
        public int Length;

        /// <summary>
        /// Specifies flags that control the position of the minimized window and the method by which the window is restored.
        /// </summary>
        public int Flags;

        /// <summary>
        /// The current show state of the window.
        /// </summary>
        public ShowWindowCommands ShowCmd;

        /// <summary>
        /// The coordinates of the window's upper-left corner when the window is minimized.
        /// </summary>
        public Point MinPosition;

        /// <summary>
        /// The coordinates of the window's upper-left corner when the window is maximized.
        /// </summary>
        public Point MaxPosition;

        /// <summary>
        /// The window's coordinates when the window is in the restored position.
        /// </summary>
        public RECT NormalPosition;

        /// <summary>
        /// Gets the default (empty) value.
        /// </summary>
        public static WINDOWPLACEMENT Default {
            get {
                WINDOWPLACEMENT result = new WINDOWPLACEMENT();
                result.Length = Marshal.SizeOf(result);
                return result;
            }
        }

        public ShowWindowCommands SetShowCmd(ShowWindowCommands windowState)
        {
            return ShowCmd = windowState;
        }

        public override string ToString()
        {
            return $"{{{Enum.GetName(typeof(ShowWindowCommands), ShowCmd)} {NormalPosition}}}";
        }
    }

    public enum ShowWindowCommands
    {
        /// <summary>
        /// Hides the window and activates another window.
        /// </summary>
        Hide = 0,
        /// <summary>
        /// Activates and displays a window. If the window is minimized or 
        /// maximized, the system restores it to its original size and position.
        /// An application should specify this flag when displaying the window 
        /// for the first time.
        /// </summary>
        Normal = 1,
        /// <summary>
        /// Activates the window and displays it as a minimized window.
        /// </summary>
        ShowMinimized = 2,
        /// <summary>
        /// Maximizes the specified window.
        /// </summary>
        Maximize = 3, // is this the right value?
        /// <summary>
        /// Activates the window and displays it as a maximized window.
        /// </summary>       
        ShowMaximized = 3,
        /// <summary>
        /// Displays a window in its most recent size and position. This value 
        /// is similar to <see cref="Win32.ShowWindowCommand.Normal"/>, except 
        /// the window is not activated.
        /// </summary>
        ShowNoActivate = 4,
        /// <summary>
        /// Activates the window and displays it in its current size and position. 
        /// </summary>
        Show = 5,
        /// <summary>
        /// Minimizes the specified window and activates the next top-level 
        /// window in the Z order.
        /// </summary>
        Minimize = 6,
        /// <summary>
        /// Displays the window as a minimized window. This value is similar to
        /// <see cref="Win32.ShowWindowCommand.ShowMinimized"/>, except the 
        /// window is not activated.
        /// </summary>
        ShowMinNoActive = 7,
        /// <summary>
        /// Displays the window in its current size and position. This value is 
        /// similar to <see cref="Win32.ShowWindowCommand.Show"/>, except the 
        /// window is not activated.
        /// </summary>
        ShowNA = 8,
        /// <summary>
        /// Activates and displays the window. If the window is minimized or 
        /// maximized, the system restores it to its original size and position. 
        /// An application should specify this flag when restoring a minimized window.
        /// </summary>
        Restore = 9,
        /// <summary>
        /// Sets the show state based on the SW_* value specified in the 
        /// STARTUPINFO structure passed to the CreateProcess function by the 
        /// program that started the application.
        /// </summary>
        ShowDefault = 10,
        /// <summary>
        ///  <b>Windows 2000/XP:</b> Minimizes a window, even if the thread 
        /// that owns the window is not responding. This flag should only be 
        /// used when minimizing windows from a different thread.
        /// </summary>
        ForceMinimize = 11
    }

    /// <summary>
    /// SetWindowPos Flags
    /// </summary>
    public static class SWP
    {
        public static readonly uint
        NOSIZE = 0x0001,
        NOMOVE = 0x0002,
        NOZORDER = 0x0004,
        NOREDRAW = 0x0008,
        NOACTIVATE = 0x0010,
        DRAWFRAME = 0x0020,
        FRAMECHANGED = 0x0020,
        SHOWWINDOW = 0x0040,
        HIDEWINDOW = 0x0080,
        NOCOPYBITS = 0x0100,
        NOOWNERZORDER = 0x0200,
        NOREPOSITION = 0x0200,
        NOSENDCHANGING = 0x0400,
        DEFERERASE = 0x2000,
        ASYNCWINDOWPOS = 0x4000;
    }

    public static class VIRTUALKEY
    {

        /*
	* Virtual Keys, Standard Set */
        public const ushort VK_LBUTTON = 0x01;
        public const ushort VK_RBUTTON = 0x02;
        public const ushort VK_CANCEL = 0x03;
        public const ushort VK_MBUTTON = 0x04;    /* NOT contiguous with L & RBUTTON */

        //#if(_WIN32_WINNT >= 0x0500)
        public const ushort VK_XBUTTON1 = 0x05;    /* NOT contiguous with L & RBUTTON */
        public const ushort VK_XBUTTON2 = 0x06;    /* NOT contiguous with L & RBUTTON */
        //#endif /* _WIN32_WINNT >= 0x0500 */

        /*
	* 0x07 : unassigned */
        public const ushort VK_BACK = 0x08;
        public const ushort VK_TAB = 0x09;

        /*
	* 0x0A - 0x0B : reserved */
        public const ushort VK_CLEAR = 0x0C;
        public const ushort VK_RETURN = 0x0D;

        public const ushort VK_SHIFT = 0x10;
        public const ushort VK_CONTROL = 0x11;
        public const ushort VK_MENU = 0x12;
        public const ushort VK_PAUSE = 0x13;
        public const ushort VK_CAPITAL = 0x14;

        public const ushort VK_KANA = 0x15;
        public const ushort VK_HANGEUL = 0x15;  /* old name - should be here for compatibility */
        public const ushort VK_HANGUL = 0x15;
        public const ushort VK_JUNJA = 0x17;
        public const ushort VK_FINAL = 0x18;
        public const ushort VK_HANJA = 0x19;
        public const ushort VK_KANJI = 0x19;

        public const ushort VK_ESCAPE = 0x1B;

        public const ushort VK_CONVERT = 0x1C;
        public const ushort VK_NONCONVERT = 0x1D;
        public const ushort VK_ACCEPT = 0x1E;
        public const ushort VK_MODECHANGE = 0x1F;

        public const ushort VK_SPACE = 0x20;
        public const ushort VK_PRIOR = 0x21;
        public const ushort VK_NEXT = 0x22;
        public const ushort VK_END = 0x23;
        public const ushort VK_HOME = 0x24;
        public const ushort VK_LEFT = 0x25;
        public const ushort VK_UP = 0x26;
        public const ushort VK_RIGHT = 0x27;
        public const ushort VK_DOWN = 0x28;
        public const ushort VK_SELECT = 0x29;
        public const ushort VK_PRushort = 0x2A;
        public const ushort VK_EXECUTE = 0x2B;
        public const ushort VK_SNAPSHOT = 0x2C;
        public const ushort VK_INSERT = 0x2D;
        public const ushort VK_DELETE = 0x2E;
        public const ushort VK_HELP = 0x2F;

        /*
		public const ushort VK_LWIN = 0x5B;CII '0' - '9' (0x30 - 0x39)
	* 0x40 : unassigned * VK_A - VK_Z are the same as ASCII 'A' - 'Z' (0x41 - 0x5A) */
        public const ushort VK_LWIN = 0x5B;
        public const ushort VK_RWIN = 0x5C;
        public const ushort VK_APPS = 0x5D;

        /*
	* 0x5E : reserved */
        public const ushort VK_SLEEP = 0x5F;

        public const ushort VK_NUMPAD0 = 0x60;
        public const ushort VK_NUMPAD1 = 0x61;
        public const ushort VK_NUMPAD2 = 0x62;
        public const ushort VK_NUMPAD3 = 0x63;
        public const ushort VK_NUMPAD4 = 0x64;
        public const ushort VK_NUMPAD5 = 0x65;
        public const ushort VK_NUMPAD6 = 0x66;
        public const ushort VK_NUMPAD7 = 0x67;
        public const ushort VK_NUMPAD8 = 0x68;
        public const ushort VK_NUMPAD9 = 0x69;
        public const ushort VK_MULTIPLY = 0x6A;
        public const ushort VK_ADD = 0x6B;
        public const ushort VK_SEPARATOR = 0x6C;
        public const ushort VK_SUBTRACT = 0x6D;
        public const ushort VK_DECIMAL = 0x6E;
        public const ushort VK_DIVIDE = 0x6F;
        public const ushort VK_F1 = 0x70;
        public const ushort VK_F2 = 0x71;
        public const ushort VK_F3 = 0x72;
        public const ushort VK_F4 = 0x73;
        public const ushort VK_F5 = 0x74;
        public const ushort VK_F6 = 0x75;
        public const ushort VK_F7 = 0x76;
        public const ushort VK_F8 = 0x77;
        public const ushort VK_F9 = 0x78;
        public const ushort VK_F10 = 0x79;
        public const ushort VK_F11 = 0x7A;
        public const ushort VK_F12 = 0x7B;
        public const ushort VK_F13 = 0x7C;
        public const ushort VK_F14 = 0x7D;
        public const ushort VK_F15 = 0x7E;
        public const ushort VK_F16 = 0x7F;
        public const ushort VK_F17 = 0x80;
        public const ushort VK_F18 = 0x81;
        public const ushort VK_F19 = 0x82;
        public const ushort VK_F20 = 0x83;
        public const ushort VK_F21 = 0x84;
        public const ushort VK_F22 = 0x85;
        public const ushort VK_F23 = 0x86;
        public const ushort VK_F24 = 0x87;

        /*
	* 0x88 - 0x8F : unassigned */
        public const ushort VK_NUMLOCK = 0x90;
        public const ushort VK_SCROLL = 0x91;

        /*
	* NEC PC-9800 kbd definitions */
        public const ushort VK_OEM_NEC_EQUAL = 0x92;   // '=' key on numpad

        /*
	* Fujitsu/OASYS kbd definitions */
        public const ushort VK_OEM_FJ_JISHO = 0x92;   // 'Dictionary' key
        public const ushort VK_OEM_FJ_MASSHOU = 0x93;   // 'Unregister word' key
        public const ushort VK_OEM_FJ_TOUROKU = 0x94;   // 'Register word' key
        public const ushort VK_OEM_FJ_LOYA = 0x95;   // 'Left OYAYUBI' key
        public const ushort VK_OEM_FJ_ROYA = 0x96;   // 'Right OYAYUBI' key

        /*
	* 0x97 - 0x9F : unassigned */
        /*
	* VK_L* & VK_R* - left and right Alt, Ctrl and Shift virtual keys. * Used only as parameters to GetAsyncKeyState() and GetKeyState(). * No other API or message will distinguish left and right keys in this way. */
        public const ushort VK_LSHIFT = 0xA0;
        public const ushort VK_RSHIFT = 0xA1;
        public const ushort VK_LCONTROL = 0xA2;
        public const ushort VK_RCONTROL = 0xA3;
        public const ushort VK_LMENU = 0xA4;
        public const ushort VK_RMENU = 0xA5;

        //#if(_WIN32_WINNT >= 0x0500)
        public const ushort VK_BROWSER_BACK = 0xA6;
        public const ushort VK_BROWSER_FORWARD = 0xA7;
        public const ushort VK_BROWSER_REFRESH = 0xA8;
        public const ushort VK_BROWSER_STOP = 0xA9;
        public const ushort VK_BROWSER_SEARCH = 0xAA;
        public const ushort VK_BROWSER_FAVORITES = 0xAB;
        public const ushort VK_BROWSER_HOME = 0xAC;

        public const ushort VK_VOLUME_MUTE = 0xAD;
        public const ushort VK_VOLUME_DOWN = 0xAE;
        public const ushort VK_VOLUME_UP = 0xAF;
        public const ushort VK_MEDIA_NEXT_TRACK = 0xB0;
        public const ushort VK_MEDIA_PREV_TRACK = 0xB1;
        public const ushort VK_MEDIA_STOP = 0xB2;
        public const ushort VK_MEDIA_PLAY_PAUSE = 0xB3;
        public const ushort VK_LAUNCH_MAIL = 0xB4;
        public const ushort VK_LAUNCH_MEDIA_SELECT = 0xB5;
        public const ushort VK_LAUNCH_APP1 = 0xB6;
        public const ushort VK_LAUNCH_APP2 = 0xB7;

        //#endif /* _WIN32_WINNT >= 0x0500 */

        /*
	* 0xB8 - 0xB9 : reserved */
        public const ushort VK_OEM_1 = 0xBA;   // ';:' for US
        public const ushort VK_OEM_PLUS = 0xBB;   // '+' any country
        public const ushort VK_OEM_COMMA = 0xBC;   // ',' any country
        public const ushort VK_OEM_MINUS = 0xBD;   // '-' any country
        public const ushort VK_OEM_PERIOD = 0xBE;   // '.' any country
        public const ushort VK_OEM_2 = 0xBF;   // '/?' for US
        public const ushort VK_OEM_3 = 0xC0;   // '`~' for US

        /*
	* 0xC1 - 0xD7 : reserved */
        /*
	* 0xD8 - 0xDA : unassigned */
        public const ushort VK_OEM_4 = 0xDB;  //  '[{' for US
        public const ushort VK_OEM_5 = 0xDC;  //  '\|' for US
        public const ushort VK_OEM_6 = 0xDD;  //  ']}' for US
        public const ushort VK_OEM_7 = 0xDE;  //  ''"' for US
        public const ushort VK_OEM_8 = 0xDF;

        /*
	* 0xE0 : reserved */
        /*
	* Various extended or enhanced keyboards */
        public const ushort VK_OEM_AX = 0xE1;  //  'AX' key on Japanese AX kbd
        public const ushort VK_OEM_102 = 0xE2;  //  "<>" or "\|" on RT 102-key kbd.
        public const ushort VK_ICO_HELP = 0xE3;  //  Help key on ICO
        public const ushort VK_ICO_00 = 0xE4;  //  00 key on ICO

        //#if(WINVER >= 0x0400)
        public const ushort VK_PROCESSKEY = 0xE5;
        //#endif /* WINVER >= 0x0400 */

        public const ushort VK_ICO_CLEAR = 0xE6;
        //#if(_WIN32_WINNT >= 0x0500)
        public const ushort VK_PACKET = 0xE7;
        //#endif /* _WIN32_WINNT >= 0x0500 */

        /*
	* 0xE8 : unassigned */
        /*
	* Nokia/Ericsson definitions */
        public const ushort VK_OEM_RESET = 0xE9;
        public const ushort VK_OEM_JUMP = 0xEA;
        public const ushort VK_OEM_PA1 = 0xEB;
        public const ushort VK_OEM_PA2 = 0xEC;
        public const ushort VK_OEM_PA3 = 0xED;
        public const ushort VK_OEM_WSCTRL = 0xEE;
        public const ushort VK_OEM_CUSEL = 0xEF;
        public const ushort VK_OEM_ATTN = 0xF0;
        public const ushort VK_OEM_FINISH = 0xF1;
        public const ushort VK_OEM_COPY = 0xF2;
        public const ushort VK_OEM_AUTO = 0xF3;
        public const ushort VK_OEM_ENLW = 0xF4;
        public const ushort VK_OEM_BACKTAB = 0xF5;

        public const ushort VK_ATTN = 0xF6;
        public const ushort VK_CRSEL = 0xF7;
        public const ushort VK_EXSEL = 0xF8;
        public const ushort VK_EREOF = 0xF9;
        public const ushort VK_PLAY = 0xFA;
        public const ushort VK_ZOOM = 0xFB;
        public const ushort VK_NONAME = 0xFC;
        public const ushort VK_PA1 = 0xFD;
        public const ushort VK_OEM_CLEAR = 0xFE;

        /*
	* 0xFF : reserved */
        /* missing letters and numbers for convenience*/
        public static ushort VK_0 = 0x30;
        public static ushort VK_1 = 0x31;
        public static ushort VK_2 = 0x32;
        public static ushort VK_3 = 0x33;
        public static ushort VK_4 = 0x34;
        public static ushort VK_5 = 0x35;
        public static ushort VK_6 = 0x36;
        public static ushort VK_7 = 0x37;
        public static ushort VK_8 = 0x38;
        public static ushort VK_9 = 0x39;
        /* 0x40 : unassigned*/
        public static ushort VK_A = 0x41;
        public static ushort VK_B = 0x42;
        public static ushort VK_C = 0x43;
        public static ushort VK_D = 0x44;
        public static ushort VK_E = 0x45;
        public static ushort VK_F = 0x46;
        public static ushort VK_G = 0x47;
        public static ushort VK_H = 0x48;
        public static ushort VK_I = 0x49;
        public static ushort VK_J = 0x4A;
        public static ushort VK_K = 0x4B;
        public static ushort VK_L = 0x4C;
        public static ushort VK_M = 0x4D;
        public static ushort VK_N = 0x4E;
        public static ushort VK_O = 0x4F;
        public static ushort VK_P = 0x50;
        public static ushort VK_Q = 0x51;
        public static ushort VK_R = 0x52;
        public static ushort VK_S = 0x53;
        public static ushort VK_T = 0x54;
        public static ushort VK_U = 0x55;
        public static ushort VK_V = 0x56;
        public static ushort VK_W = 0x57;
        public static ushort VK_X = 0x58;
        public static ushort VK_Y = 0x59;
        public static ushort VK_Z = 0x5A;
    }

    public enum EMouseFlags : int
    {
        MoveAndAbsolute = 0x0001 | 0x8000,
        LeftDown = 0x0002,
        LeftUp = 0x0004,
        LeftClick = LeftDown | LeftUp,
        RightDown = 0x0008,
        RightUp = 0x0010,
        RightClick = RightDown | RightUp
    }

    [Flags]
    public enum CompositionAction : uint
    {
        /// <summary>
        /// To enable DWM composition
        /// </summary>
        DWM_EC_DISABLECOMPOSITION = 0,
        /// <summary>
        /// To disable composition.
        /// </summary>
        DWM_EC_ENABLECOMPOSITION = 1
    }

    public static class WINEVENT
    {
        public const uint WINEVENT_OUTOFCONTEXT = 0x0000; // Events are ASYNC

        public const uint WINEVENT_SKIPOWNTHREAD = 0x0001; // Don't call back for events on installer's thread

        public const uint WINEVENT_SKIPOWNPROCESS = 0x0002; // Don't call back for events on installer's process

        public const uint WINEVENT_INCONTEXT = 0x0004; // Events are SYNC, this causes your dll to be injected into every process

        public const uint EVENT_MIN = 0x00000001;

        public const uint EVENT_MAX = 0x7FFFFFFF;

        public const uint EVENT_SYSTEM_SOUND = 0x0001;

        public const uint EVENT_SYSTEM_ALERT = 0x0002;

        public const uint EVENT_SYSTEM_FOREGROUND = 0x0003;

        public const uint EVENT_SYSTEM_MENUSTART = 0x0004;

        public const uint EVENT_SYSTEM_MENUEND = 0x0005;

        public const uint EVENT_SYSTEM_MENUPOPUPSTART = 0x0006;

        public const uint EVENT_SYSTEM_MENUPOPUPEND = 0x0007;

        public const uint EVENT_SYSTEM_CAPTURESTART = 0x0008;

        public const uint EVENT_SYSTEM_CAPTUREEND = 0x0009;

        public const uint EVENT_SYSTEM_MOVESIZESTART = 0x000A;

        public const uint EVENT_SYSTEM_MOVESIZEEND = 0x000B;

        public const uint EVENT_SYSTEM_CONTEXTHELPSTART = 0x000C;

        public const uint EVENT_SYSTEM_CONTEXTHELPEND = 0x000D;

        public const uint EVENT_SYSTEM_DRAGDROPSTART = 0x000E;

        public const uint EVENT_SYSTEM_DRAGDROPEND = 0x000F;

        public const uint EVENT_SYSTEM_DIALOGSTART = 0x0010;

        public const uint EVENT_SYSTEM_DIALOGEND = 0x0011;

        public const uint EVENT_SYSTEM_SCROLLINGSTART = 0x0012;

        public const uint EVENT_SYSTEM_SCROLLINGEND = 0x0013;

        public const uint EVENT_SYSTEM_SWITCHSTART = 0x0014;

        public const uint EVENT_SYSTEM_SWITCHEND = 0x0015;

        public const uint EVENT_SYSTEM_MINIMIZESTART = 0x0016;

        public const uint EVENT_SYSTEM_MINIMIZEEND = 0x0017;

        public const uint EVENT_SYSTEM_DESKTOPSWITCH = 0x0020;

        public const uint EVENT_SYSTEM_END = 0x00FF;

        public const uint EVENT_OEM_DEFINED_START = 0x0101;

        public const uint EVENT_OEM_DEFINED_END = 0x01FF;

        public const uint EVENT_UIA_EVENTID_START = 0x4E00;

        public const uint EVENT_UIA_EVENTID_END = 0x4EFF;

        public const uint EVENT_UIA_PROPID_START = 0x7500;

        public const uint EVENT_UIA_PROPID_END = 0x75FF;

        public const uint EVENT_CONSOLE_CARET = 0x4001;

        public const uint EVENT_CONSOLE_UPDATE_REGION = 0x4002;

        public const uint EVENT_CONSOLE_UPDATE_SIMPLE = 0x4003;

        public const uint EVENT_CONSOLE_UPDATE_SCROLL = 0x4004;

        public const uint EVENT_CONSOLE_LAYOUT = 0x4005;

        public const uint EVENT_CONSOLE_START_APPLICATION = 0x4006;

        public const uint EVENT_CONSOLE_END_APPLICATION = 0x4007;

        public const uint EVENT_CONSOLE_END = 0x40FF;

        public const uint EVENT_OBJECT_CREATE = 0x8000; // hwnd ID idChild is created item

        public const uint EVENT_OBJECT_DESTROY = 0x8001; // hwnd ID idChild is destroyed item

        public const uint EVENT_OBJECT_SHOW = 0x8002; // hwnd ID idChild is shown item

        public const uint EVENT_OBJECT_HIDE = 0x8003; // hwnd ID idChild is hidden item

        public const uint EVENT_OBJECT_REORDER = 0x8004; // hwnd ID idChild is parent of zordering children

        public const uint EVENT_OBJECT_FOCUS = 0x8005; // hwnd ID idChild is focused item

        public const uint EVENT_OBJECT_SELECTION = 0x8006; // hwnd ID idChild is selected item (if only one), or idChild is OBJID_WINDOW if complex

        public const uint EVENT_OBJECT_SELECTIONADD = 0x8007; // hwnd ID idChild is item added

        public const uint EVENT_OBJECT_SELECTIONREMOVE = 0x8008; // hwnd ID idChild is item removed

        public const uint EVENT_OBJECT_SELECTIONWITHIN = 0x8009; // hwnd ID idChild is parent of changed selected items

        public const uint EVENT_OBJECT_STATECHANGE = 0x800A; // hwnd ID idChild is item w/ state change

        public const uint EVENT_OBJECT_LOCATIONCHANGE = 0x800B; // hwnd ID idChild is moved/sized item

        public const uint EVENT_OBJECT_NAMECHANGE = 0x800C; // hwnd ID idChild is item w/ name change

        public const uint EVENT_OBJECT_DESCRIPTIONCHANGE = 0x800D; // hwnd ID idChild is item w/ desc change

        public const uint EVENT_OBJECT_VALUECHANGE = 0x800E; // hwnd ID idChild is item w/ value change

        public const uint EVENT_OBJECT_PARENTCHANGE = 0x800F; // hwnd ID idChild is item w/ new parent

        public const uint EVENT_OBJECT_HELPCHANGE = 0x8010; // hwnd ID idChild is item w/ help change

        public const uint EVENT_OBJECT_DEFACTIONCHANGE = 0x8011; // hwnd ID idChild is item w/ def action change

        public const uint EVENT_OBJECT_ACCELERATORCHANGE = 0x8012; // hwnd ID idChild is item w/ keybd accel change

        public const uint EVENT_OBJECT_INVOKED = 0x8013; // hwnd ID idChild is item invoked

        public const uint EVENT_OBJECT_TEXTSELECTIONCHANGED = 0x8014; // hwnd ID idChild is item w? test selection change

        public const uint EVENT_OBJECT_CONTENTSCROLLED = 0x8015;

        public const uint EVENT_SYSTEM_ARRANGMENTPREVIEW = 0x8016;

        public const uint EVENT_OBJECT_END = 0x80FF;

        public const uint EVENT_AIA_START = 0xA000;

        public const uint EVENT_AIA_END = 0xAFFF;
    }
}
