﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public class TypeNamePair
    {
        public Type Type { get; private set; }
        public string Name { get; private set; }

        public TypeNamePair(Type type, string name) {
            Type = type;
            Name = name;
        }
    }
}
