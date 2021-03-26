// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Teronis.Extensions;

namespace Teronis.Windows.Forms.Hooks
{
    public class KeyboardHookManager
    {
        readonly KeyboardHook keyboardHook;
        readonly IDictionary<int, TParentHotkeyInvoker> registeredHotkeys;
        TParentHotkeyInvoker pressedHotkey = null;

        public KeyboardHookManager(System.Reflection.Assembly executingAssembly)
        {
            keyboardHook = new KeyboardHook(executingAssembly);
            keyboardHook.KeyDown += KeyboardHook_KeyDown;
            registeredHotkeys = new Dictionary<int, TParentHotkeyInvoker>();
            pressedHotkey = null;
        }

        /// <summary>
        /// Registers a hotkey. To register only a modifier: set it as <see cref="TKeyEventArgs.KeyCode"/>.
        /// </summary>
        /// <param name="args"></param>
        /// <returns>Hotkey id. You have to unregister it.</returns>
        public TChildHotkey RegisterHotkey(TKeyEventArgs args)
        {
            TParentHotkeyInvoker hotkeyInvoker = null;

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

                hotkeyInvoker = TParentHotkeyInvoker.Create(id, args, (childHotkey) => UnregisterHotkey(id));
                registeredHotkeys.Add(id, hotkeyInvoker);
                return hotkeyInvoker.RegisterChildren();
            } else {
                hotkeyInvoker.Counter++;
                return hotkeyInvoker.RegisterChildren();
            }
        }

        public void UnregisterHotkey(int registeredHotkeyId)
        {
            if (registeredHotkeys.TryGetValue(registeredHotkeyId, out TParentHotkeyInvoker hotkeyInvoker) && hotkeyInvoker.Counter == 1) {
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

        private void KeyboardHook_KeyDown(object sender, KeyEventArgs args)
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
                pressedHotkey.Invoke(EHotkeyState.Down);
                keyboardHook.KeyDown -= KeyboardHook_KeyDown;
                keyboardHook.KeyUp += KeyboardHook_KeyUp;
            }
        }

        private void KeyboardHook_KeyUp(object sender, KeyEventArgs e)
        {
            //Console.WriteLine($"UP control: {e.Control} alt: {e.Alt} shift: {e.Shift} keycode: {e.KeyCode} keydata: {e.KeyData} modifiers: {e.Modifiers} suppressKeyPress: {e.SuppressKeyPress}");

            if (pressedHotkey == null) {
                return;
            }

            bool isOnlyModifier()
            {
                return pressedHotkey.KeyEventArgs.Modifiers == EKeyboardModifier.Control && (e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey)
                    || pressedHotkey.KeyEventArgs.Modifiers == EKeyboardModifier.Shift && (e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey)
                    || (pressedHotkey.KeyEventArgs.Modifiers & EKeyboardModifier.Alt) == EKeyboardModifier.Alt && (e.KeyCode == Keys.LMenu || e.KeyCode == Keys.RMenu)
                    || pressedHotkey.KeyEventArgs.Modifiers == EKeyboardModifier.None;
            }

            if (isOnlyModifier()) {
                releaseKeyDown();
            }
        }

        private void releaseKeyDown()
        {
            var _pressedHotkey = pressedHotkey;
            pressedHotkey = null;
            _pressedHotkey.Invoke(EHotkeyState.Up);
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

        private class TParentHotkeyInvoker : TParentHotkey
        {
            public static TParentHotkeyInvoker Create(int id, TKeyEventArgs keyEventArgs, Action<TChildHotkey> unregister)
            {
                var hotkeyInvoker = new TParentHotkeyInvoker(id, keyEventArgs, out Action<EHotkeyState> invokeHotkey, unregister) {
                    Invoke = invokeHotkey
                };

                return hotkeyInvoker;
            }

            public Action<EHotkeyState> Invoke { get; private set; }
            public int Counter = 1;

            private TParentHotkeyInvoker(int id, TKeyEventArgs keyEventArgs, out Action<EHotkeyState> invokeHotkeyFired, Action<TChildHotkey> unregister) : base(id, keyEventArgs, out invokeHotkeyFired, unregister) { }

            public TChildHotkey RegisterChildren()
            {
                return registerChildren(this);
            }
        }
    }

    public class TKeyEventArgs
    {
        public EKeyboardModifier Modifiers;
        public Keys KeyCode;
    }

    public class TParentHotkey
    {
        public int Id { get; private set; }
        public TKeyEventArgs KeyEventArgs { get; private set; }

        readonly Action<TChildHotkey> unregister;
        readonly List<TChildHotkeyInvoker> hotkeys;

        public TParentHotkey(int id, TKeyEventArgs keyEventArgs, out Action<EHotkeyState> invokeHotkeyFired, Action<TChildHotkey> unregister)
        {
            Id = id;
            KeyEventArgs = keyEventArgs;
            invokeHotkeyFired = this.invokeHotkeyFired;
            this.unregister = unregister;
            hotkeys = new List<TChildHotkeyInvoker>();
        }

        /// <summary>
        /// It invokes one of the two events above.
        /// </summary>
        /// <param name="hotkeyState">false=down; true=up;</param>
        private void invokeHotkeyFired(EHotkeyState hotkeyState)
        {
            foreach (var hotkey in hotkeys) {
                if (hotkeyState == EHotkeyState.Down) {
                    hotkey.InvokeHotkeyDownFired();
                } else {
                    hotkey.InvokeHotkeyUpFired();
                }
            }
        }

        protected TChildHotkey registerChildren(TParentHotkey parentHotkey)
        {
            var childHotkey = new TChildHotkeyInvoker(parentHotkey, _unregister) { };
            hotkeys.Add(childHotkey);
            return childHotkey;
        }

        private void _unregister(TChildHotkey childHotkey)
        {
            hotkeys.Remove((TChildHotkeyInvoker)childHotkey);
#if DEBUG
            Console.WriteLine($"Hotkey [{childHotkey.ParentHotkey.KeyEventArgs.Modifiers}] + [{childHotkey.ParentHotkey.KeyEventArgs.KeyCode}] with id {childHotkey.ParentHotkey.Id} deleted.");
#endif
            unregister(childHotkey);
        }

        private class TChildHotkeyInvoker : TChildHotkey
        {
            public TChildHotkeyInvoker(TParentHotkey parentHotkey, Action<TChildHotkey> unregister) : base(unregister)
            {
                ParentHotkey = parentHotkey;
            }

            public void InvokeHotkeyDownFired() => invokeHotkeyDownFired();

            public void InvokeHotkeyUpFired() => invokeHotkeyUpFired();
        }
    }

    public class TChildHotkey
    {
        public event Action HotkeyDownFired;
        public event Action HotkeyUpFired;

        public TParentHotkey ParentHotkey;
        readonly Action<TChildHotkey> unregister;

        public TChildHotkey(Action<TChildHotkey> unregister)
        {
            this.unregister = unregister;
        }

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

    public enum EHotkeyState
    {
        Down,
        Up
    }

    [Flags]
    public enum EKeyboardModifier
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
