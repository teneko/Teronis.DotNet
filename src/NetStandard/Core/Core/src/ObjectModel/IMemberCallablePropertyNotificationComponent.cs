// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace Teronis.ComponentModel
{
    public interface IMemberCallablePropertyChangeComponent
    {
        void OnPropertyChanged([CallerMemberName] string? propertyName = null);
        void OnPropertyChanging([CallerMemberName] string? propertyName = null);
    }
}
