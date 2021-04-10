// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.ComponentModel.Parenthood
{
    public class HavingParentsEventArgs : EventArgs
    {
        public object OriginalSource { get; private set; }
        public ParentsContainer Container { get; private set; }

        public HavingParentsEventArgs(object originalSource, Type? wantedType)
        {
            OriginalSource = originalSource;
            Container = new ParentsContainer(wantedType);
        }
    }
}
