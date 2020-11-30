using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Teronis.Windows.Hooks;

namespace Teronis.Windows.Forms.Hooks
{
    /// <summary>
	/// Captures global mouse events
	/// </summary>
	public class MouseHook : GlobalHook
    {
        public event MouseEventHandler MouseDown;
        public event MouseEventHandler MouseUp;
        public event MouseEventHandler MouseMove;
        public event MouseEventHandler MouseWheel;

        public event EventHandler Click;
        public event EventHandler DoubleClick;

        private bool? wasStarted;

        public MouseHook(System.Reflection.Assembly executingAssembly) : base(executingAssembly) => _hookType = WH_MOUSE_LL;

        protected override int HookCallbackProcedure(int nCode, int wParam, IntPtr lParam)
        {
            wasStarted = IsStarted;
            if (nCode > -1 && (MouseDown != null || MouseUp != null || MouseMove != null)) {
                if ((bool)wasStarted) {
                    Stop();
                }

                MouseLLHookStruct mouseHookStruct = (MouseLLHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseLLHookStruct));

                MouseButtons button = GetButton(wParam);
                MouseEventType eventType = GetEventType(wParam);

                MouseEventArgs e = new MouseEventArgs(
                     button,
                     eventType == MouseEventType.DoubleClick ? 2 : 1,
                     mouseHookStruct.pt.x,
                     mouseHookStruct.pt.y,
                     eventType == MouseEventType.MouseWheel ? (short)(mouseHookStruct.mouseData >> 16 & 0xffff) : 0);

                // Prevent multiple Right Click events (this probably happens for popup menus)
                if (button == MouseButtons.Right && mouseHookStruct.flags != 0) {
                    eventType = MouseEventType.None;
                }

                switch (eventType) {
                    case MouseEventType.MouseDown:
                        MouseDown?.Invoke(this, e);
                        break;
                    case MouseEventType.MouseUp:
                        Click?.Invoke(this, new EventArgs());
                        MouseUp?.Invoke(this, e);
                        break;
                    case MouseEventType.DoubleClick:
                        DoubleClick?.Invoke(this, new EventArgs());
                        break;
                    case MouseEventType.MouseWheel:
                        MouseWheel?.Invoke(this, e);
                        break;
                    case MouseEventType.MouseMove:
                        MouseMove?.Invoke(this, e);
                        break;
                    default:
                        break;
                }

                if ((bool)wasStarted) {
                    Start();
                }
            }
            wasStarted = null;
            return CallNextHookEx(_handleToHook, nCode, wParam, lParam);
        }

        public void Break()
        {
            if (wasStarted == null && IsStarted) {
                wasStarted = IsStarted;
                Stop();
            }
        }

        public void Continue()
        {
            if (wasStarted != null && (bool)wasStarted) {
                wasStarted = null;
                Start();
            }
        }

        private MouseButtons GetButton(int wParam)
        {
            switch (wParam) {

                case WM_LBUTTONDOWN:
                case WM_LBUTTONUP:
                case WM_LBUTTONDBLCLK:
                    return MouseButtons.Left;
                case WM_RBUTTONDOWN:
                case WM_RBUTTONUP:
                case WM_RBUTTONDBLCLK:
                    return MouseButtons.Right;
                case WM_MBUTTONDOWN:
                case WM_MBUTTONUP:
                case WM_MBUTTONDBLCLK:
                    return MouseButtons.Middle;
                default:
                    return MouseButtons.None;
            }
        }

        private MouseEventType GetEventType(int wParam)
        {
            switch (wParam) {
                case WM_LBUTTONDOWN:
                case WM_RBUTTONDOWN:
                case WM_MBUTTONDOWN:
                    return MouseEventType.MouseDown;
                case WM_LBUTTONUP:
                case WM_RBUTTONUP:
                case WM_MBUTTONUP:
                    return MouseEventType.MouseUp;
                case WM_LBUTTONDBLCLK:
                case WM_RBUTTONDBLCLK:
                case WM_MBUTTONDBLCLK:
                    return MouseEventType.DoubleClick;
                case WM_MOUSEWHEEL:
                    return MouseEventType.MouseWheel;
                case WM_MOUSEMOVE:
                    return MouseEventType.MouseMove;
                default:
                    return MouseEventType.None;
            }
        }

        private enum MouseEventType
        {
            None,
            MouseDown,
            MouseUp,
            DoubleClick,
            MouseWheel,
            MouseMove
        }
    }
}
