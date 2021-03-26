// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;

namespace Teronis
{
    public interface ICovariantTuple<out T1, out T2> : IComparable, IStructuralComparable, IStructuralEquatable
    {
        T1 Item1 { get; }
        T2 Item2 { get; }
    }
}
