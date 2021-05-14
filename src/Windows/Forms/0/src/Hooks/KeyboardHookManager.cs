// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Teronis.Windows.Forms.Hooks
{
    public class KeyboardHookManager
    {
        private readonly KeyboardHook keyboardHook;
        private readonly IDictionary<int, ParentHotkeyInvoker> registeredHotkeys;
        private ParentHotkeyInvoker? pressedHotkey;

        public KeyboardHookManager(System.Reflection.Assembly executingAssembly)
        {
            keyboardHook = new KeyboardHook(executingAssembly);
            keyboardHook.KeyDown += KeyboardHook_KeyDown;
            registeredHotkeys = new Dictionary<int, ParentHotkeyInvoker>();
            pressedHotkey = null;
        }

        /// <summary>
        /// Registers a hotkey. To register only a modifier: set it as <see cref="KeyEventArgs.KeyCode"/>.
        /// </summary>
        /// <param name="args"></param>
        /// <returns>Hotkey id. You have to unregister it.</returns>
        public ChildHotkey RegisterHotkey(KeyEventArgs args)
        {
            ParentHotkeyInvoker? hotkeyInvoker = null;

            if (!registeredHotkeys.Any((x) => {
                if (x.Value.KeyEventArgs.Modifiers == args.Modifiers && x.Value.KeyEventArgs.KeyCode == args.KeyCode) {
                    hotkeyInvoker = x.Value;
                    return true;
                }
                //
                return false;
            })) {
                if (registeredHotkeys.Count == 0) {
                    start();
                }

                int id = 0;

                do {
                    id++;
                } while (registeredHotkeys.Any(x => x.Value.Id == id));

                hotkeyInvoker = ParentHotkeyInvoker.Create(id, args, (childHotkey) => UnregisterHotkey(id));
                registeredHotkeys.Add(id, hotkeyInvoker);
                return hotkeyInvoker.RegisterChildren();
            } else {
                hotkeyInvoker.Counter++;
                return hotkeyInvoker.RegisterChildren();
            }
        }

        public void UnregisterHotkey(int registeredHotkeyId)
        {
            if (registeredHotkeys.TryGetValue(registeredHotkeyId, out ParentHotkeyInvoker hotkeyInvoker) && hotkeyInvoker.Counter == 1) {
                registeredHotkeys.Remove(registeredHotkeyId);

                if (registeredHotkeys.Count == 0) {
                    stop();
                }
            } else {
                if (hotkeyInvoker != null) {
                    hotkeyInvoker.Counter--;
                }
            }
        }

        private void KeyboardHook_KeyDown(object sender, System.Windows.Forms.KeyEventArgs args)
        {
            //Console.WriteLine($"DOWN control: {args.Control} alt: {args.Alt} shift: {args.Shift} keycode: {args.KeyCode} keydata: {args.KeyData} modifiers: {args.Modifiers} suppressKeyPress: {args.SuppressKeyPress}");

            if (pressedHotkey != null) {
                return;
            }

            bool doesRegisteredHotkeysHaveAny()
            {
                return registeredHotkeys.Any(x => {
                    var keyArgs = x.Value.KeyEventArgs;

                    //if (Enum.TryParse(Enum.GetName(typeof(EKeyboardModifier), keyArgs.Modifier), out Keys modifier)) {
                    if ((ushort)args.KeyData == (ushort)keyArgs.KeyCode && (int)args.Modifiers == /*modifier*/(int)keyArgs.Modifiers) {
                        pressedHotkey = x.Value;
                        return true;
                    }
                    //}
                    //
                    return false;
                });
            }

            if (doesRegisteredHotkeysHaveAny()) {
                pressedHotkey.Invoke(HotkeyState.Down);
                keyboardHook.KeyDown -= KeyboardHook_KeyDown;
                keyboardHook.KeyUp += KeyboardHook_KeyUp;
            }
        }

        private void KeyboardHook_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //Console.WriteLine($"UP control: {e.Control} alt: {e.Alt} shift: {e.Shift} keycode: {e.KeyCode} keydata: {e.KeyData} modifiers: {e.Modifiers} suppressKeyPress: {e.SuppressKeyPress}");

            if (pressedHotkey == null) {
                return;
            }

            bool isOnlyModifier()
            {
                return pressedHotkey.KeyEventArgs.Modifiers == KeyboardModifier.Control && (e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey)
                    || pressedHotkey.KeyEventArgs.Modifiers == KeyboardModifier.Shift && (e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey)
                    || (pressedHotkey.KeyEventArgs.Modifiers & KeyboardModifier.Alt) == KeyboardModifier.Alt && (e.KeyCode == Keys.LMenu || e.KeyCode == Keys.RMenu)
                    || pressedHotkey.KeyEventArgs.Modifiers == KeyboardModifier.None;
            }

            if (isOnlyModifier()) {
                releaseKeyDown();
            }
        }

        private void releaseKeyDown()
        {
            var _pressedHotkey = pressedHotkey;
            pressedHotkey = null;
            _pressedHotkey.Invoke(HotkeyState.Up);
            keyboardHook.KeyUp -= KeyboardHook_KeyUp;
            keyboardHook.KeyDown += KeyboardHook_KeyDown;
        }

        private void start()
        {
            if (!keyboardHook.IsStarted) {
                keyboardHook.Start();
            }
        }

        private void stop()
        {
            if (keyboardHook.IsStarted) {
                keyboardHook.Stop();
            }
        }

        private class ParentHotkeyInvoker : ParentHotkey
        {
            public static ParentHotkeyInvoker Create(int id, KeyEventArgs keyEventArgs, Action<ChildHotkey> unregister)
            {
                var hotkeyInvoker = new ParentHotkeyInvoker(id, keyEventArgs, out Action<HotkeyState> invokeHotkey, unregister) {
                    Invoke = invokeHotkey
                };

                return hotkeyInvoker;
            }

            public Action<HotkeyState> Invoke { get; private set; }
            public int Counter = 1;

            private ParentHotkeyInvoker(int id, KeyEventArgs keyEventArgs, out Action<HotkeyState> invokeHotkeyFired, Action<ChildHotkey> unregister) 
                : base(id, keyEventArgs, out invokeHotkeyFired, unregister) { }

            public ChildHotkey RegisterChildren() =>
                registerChildren(this);
        }
    }

    public class KeyEventArgs
    {
        public KeyboardModifier Modifiers;
        public Keys KeyCode;
    }

    public class ParentHotkey
    {
        public int Id { get; private set; }
        public KeyEventArgs KeyEventArgs { get; private set; }

        readonly Action<ChildHotkey> unregister;
        readonly List<ChildHotkeyInvoker> hotkeys;

        public ParentHotkey(int id, KeyEventArgs keyEventArgs, out Action<HotkeyState> invokeHotkeyFired, Action<ChildHotkey> unregister)
        {
            Id = id;
            KeyEventArgs = keyEventArgs;
            invokeHotkeyFired = this.invokeHotkeyFired;
            this.unregister = unregister;
            hotkeys = new List<ChildHotkeyInvoker>();
        }

        /// <summary>
        /// It invokes one of the two events above.
        /// </summary>
        /// <param name="hotkeyState">false=down; true=up;</param>
        private void invokeHotkeyFired(HotkeyState hotkeyState)
        {
            foreach (var hotkey in hotkeys) {
                if (hotkeyState == HotkeyState.Down) {
                    hotkey.InvokeHotkeyDownFired();
                } else {
                    hotkey.InvokeHotkeyUpFired();
                }
            }
        }

        protected ChildHotkey registerChildren(ParentHotkey parentHotkey)
        {
            var childHotkey = new ChildHotkeyInvoker(parentHotkey, _unregister) { };
            hotkeys.Add(childHotkey);
            return childHotkey;
        }

        private void _unregister(ChildHotkey childHotkey)
        {
            hotkeys.Remove((ChildHotkeyInvoker)childHotkey);
#if DEBUG
            Console.WriteLine($"Hotkey [{childHotkey.ParentHotkey.KeyEventArgs.Modifiers}] + [{childHotkey.ParentHotkey.KeyEventArgs.KeyCode}] with id {childHotkey.ParentHotkey.Id} deleted.");
#endif
            unregister(childHotkey);
        }

        private class ChildHotkeyInvoker : ChildHotkey
        {
            public ChildHotkeyInvoker(ParentHotkey parentHotkey, Action<ChildHotkey> unregister) : base(unregister)
            {
                ParentHotkey = parentHotkey;
            }

            public void InvokeHotkeyDownFired() => invokeHotkeyDownFired();

            public void InvokeHotkeyUpFired() => invokeHotkeyUpFired();
        }
    }

    public class ChildHotkey
    {
        public event Action? HotkeyDownFired;
        public event Action? HotkeyUpFired;

        public ParentHotkey? ParentHotkey;
        readonly Action<ChildHotkey> unregister;

        public ChildHotkey(Action<ChildHotkey> unregister) =>
            this.unregister = unregister;

        public void Unregister() => unregister(this);

        protected void invokeHotkeyUpFired()
        {
            HotkeyUpFired?.Invoke();
        }

        protected void invokeHotkeyDownFired()
        {
            HotkeyDownFired?.Invoke();
        }
    }

    public enum HotkeyState
    {
        Down,
        Up
    }

    [Flags]
    public enum KeyboardModifier
    {
        None = Keys.None,
        //
        // Zusammenfassung:
        //     Die linke oder rechte ALT-Modifizierertaste.
        Alt = Keys.Alt,
        //
        // Zusammenfassung:
        //     Die linke oder Rechte Modifizierer UMSCHALTTASTE.
        Shift = Keys.Shift,
        //
        // Zusammenfassung:
        //     Die linke oder rechte STRG Modifizierertaste.
        Control = Keys.Control
    }
}
