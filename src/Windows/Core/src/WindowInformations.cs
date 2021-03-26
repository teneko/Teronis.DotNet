// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Drawing;

namespace Teronis.Windows
{
    public class WindowInformations
    {
        public Rectangle InnerWinRect { get; private set; }
        public int Style { get; private set; }

        public WINDOWPLACEMENT Placement;
        readonly IntPtr hWnd;

        public WindowInformations(IntPtr hWnd)
        {
            this.hWnd = hWnd;
            Placement = new WINDOWPLACEMENT();
            Win32.GetWindowPlacement(hWnd, ref Placement);
            InnerWinRect = Win32.GetInnerWindowRectangle(hWnd);
            Style = Win32.GetWindowLong(hWnd, Win32.GWL_STYLE);
        }

        public WindowInformations(IntPtr hWnd, WINDOWPLACEMENT placement, Rectangle innerWinRect, int style)
        {
            this.hWnd = hWnd;
            Placement = placement;
            InnerWinRect = innerWinRect;
            Style = style;
        }

        public const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;
        public const int SC_MINIMIZE = 0xF020;
        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_SIZE = 0xF000;

        public void Apply()
        {
            // remove buttons
            //IntPtr sysMenu = Win32.GetSystemMenu(hWnd, false);
            //Win32.DeleteMenu(sysMenu, SC_CLOSE, MF_BYCOMMAND);
            //Win32.DeleteMenu(sysMenu, SC_MINIMIZE, MF_BYCOMMAND);
            //Win32.DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
            //Win32.DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);

            ////get item count
            //int count = Win32.GetMenuItemCount(HMENU);
            ////loop & remove
            //for (int i = 0; i < count; i++)
            //	Win32.RemoveMenu(HMENU, 0, (Win32.MF_BYPOSITION | Win32.MF_REMOVE));

            Win32.SetWindowLong(hWnd, Win32.GWL_STYLE, Style);
            Win32.SetWindowPos(hWnd, IntPtr.Zero, Placement.NormalPosition.GetX(), Placement.NormalPosition.GetY(), Placement.NormalPosition.GetWidth(), Placement.NormalPosition.GetHeight(), SWP.NOACTIVATE);

            ////force a redraw
            //Win32.DrawMenuBar(hWnd);
        }

        //public void ApplyLocation()
        //{
        //	Win32.SetWindowPos(hWnd,
        //		IntPtr.Zero, Placement.NormalPosition.GetX(),
        //		Placement.NormalPosition.GetY(),
        //		Placement.NormalPosition.GetWidth(),
        //		Placement.NormalPosition.GetHeight(),
        //		SWP.NOACTIVATE);
        //}

        public override string ToString() => $"{{{InnerWinRect} {Placement}}}";
    }
}
