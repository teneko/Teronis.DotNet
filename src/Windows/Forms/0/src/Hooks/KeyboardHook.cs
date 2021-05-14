// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Teronis.Windows.Hooks;

namespace Teronis.Windows.Forms.Hooks
{
    /// <summary>
    /// Captures global keyboard events
    /// </summary>
    public class KeyboardHook : GlobalHook
    {
        public event KeyEventHandler KeyDown;
        public event KeyEventHandler KeyUp;
        public event KeyPressEventHandler KeyPress;

        public KeyboardHook(System.Reflection.Assembly executingAssembly) : base(executingAssembly) => _hookType = WH_KEYBOARD_LL;

        protected override int HookCallbackProcedure(int nCode, int wParam, IntPtr lParam)
        {

            bool handled = false;

            if (nCode > -1 && (KeyDown != null || KeyUp != null || KeyPress != null)) {
                var keyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));

                // Is Control being held down?
                bool control = (GetKeyState(VK_LCONTROL) & 0x80) != 0 ||
                               (GetKeyState(VK_RCONTROL) & 0x80) != 0;

                // Is Shift being held down?
                bool shift = (GetKeyState(VK_LSHIFT) & 0x80) != 0 ||
                             (GetKeyState(VK_RSHIFT) & 0x80) != 0;

                // Is Alt being held down?
                bool alt = (GetKeyState(VK_LALT) & 0x80) != 0 ||
                           (GetKeyState(VK_RALT) & 0x80) != 0;

                // Is CapsLock on?
                bool capslock = GetKeyState(VK_CAPITAL) != 0;

                // Create event using keycode and control/shift/alt values found above
                System.Windows.Forms.KeyEventArgs e = new System.Windows.Forms.KeyEventArgs(
                    (Keys)(
                        keyboardHookStruct.vkCode |
                        (control ? (int)Keys.Control : 0) |
                        (shift ? (int)Keys.Shift : 0) |
                        (alt ? (int)Keys.Alt : 0)
                        ));

                // Handle KeyDown and KeyUp events
                switch (wParam) {

                    case WM_KEYDOWN:
                    case WM_SYSKEYDOWN:
                        if (KeyDown != null) {
                            KeyDown(this, e);
                            handled = handled || e.Handled;
                        }
                        break;
                    case WM_KEYUP:
                    case WM_SYSKEYUP:
                        if (KeyUp != null) {
                            KeyUp(this, e);
                            handled = handled || e.Handled;
                        }
                        break;

                }

                // Handle KeyPress event
                if (wParam == WM_KEYDOWN &&
                   !handled &&
                   !e.SuppressKeyPress &&
                    KeyPress != null) {

                    byte[] keyState = new byte[256];
                    byte[] inBuffer = new byte[2];
                    GetKeyboardState(keyState);

                    if (ToAscii(keyboardHookStruct.vkCode,
                              keyboardHookStruct.scanCode,
                              keyState,
                              inBuffer,
                              keyboardHookStruct.flags) == 1) {

                        char key = (char)inBuffer[0];
                        if (capslock ^ shift && char.IsLetter(key)) {
                            key = char.ToUpper(key);
                        }

                        KeyPressEventArgs e2 = new KeyPressEventArgs(key);
                        KeyPress(this, e2);
                        handled = handled || e.Handled;

                    }

                }

            }

            if (handled) {
                return 1;
            } else {
                return CallNextHookEx(_handleToHook, nCode, wParam, lParam);
            }

        }
    }
}
