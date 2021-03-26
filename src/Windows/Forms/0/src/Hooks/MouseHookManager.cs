// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Windows.Forms.Hooks
{
    public class MouseHookManager : MouseHook
    {
        int counter;

        public MouseHookManager(System.Reflection.Assembly executingAssembly) : base(executingAssembly) { }

        public new void Start()
        {
            counter++;
            base.Start(); // has own isStarted-property
        }

        public new void Stop()
        {
            if (counter > 0) {
                counter--;
            } else if (counter == 0) {
#if DEBUG
                Console.WriteLine("[ERROR] There are more Stop() than Start() calls!!");
#endif
                //
                base.Stop(); // has own isStarted-property
            }
        }
    }
}
