// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace Teronis.ComponentModel
{
    public interface IEventInvocablePropertyChangeComponent
    {
        void OnPropertyChanged(object? sender, PropertyChangedEventArgs args);
        void OnPropertyChanging(object? sender, PropertyChangingEventArgs args);
    }
}
