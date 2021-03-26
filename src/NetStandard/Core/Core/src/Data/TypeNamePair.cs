// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Data
{
    public class TypeNamePair
    {
        public Type Type { get; private set; }
        public string Name { get; private set; }

        public TypeNamePair(Type type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}
