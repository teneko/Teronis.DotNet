// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;

namespace Teronis.Configuration
{
    public interface ICachedSettingsPropertyValue : INotifyPropertyChanged
    {
        string PropertyName { get; }
        object? PropertyValue { get; set; }
        object? CachedPropertyValue { get; }
        bool IsCacheSynchronous { get; }
        Action<string>? NotifySettingsAboutPropertyChange { get; set; }

        void ResetCachedValue();
        void RefreshCachedValue();

        /// <summary>
        /// Notifies <see cref="settings"/> about changes from settings property "<see cref="PropertyName"/>".
        /// </summary>
        void TriggerSettingsPropertyChanged();

        /// <summary>
        /// It removes property value from cache and notifies <see cref="settings"/> about changes from settings property "<see cref="PropertyName"/>".
        /// </summary>
        void SetOriginalFromDisk();
        void SetOriginalFromCache();
        void SetOriginalFromDefault();

        /// <summary>
        /// Reminder: <see cref="CachedPropertyValue"/> gets renewed by calling <see cref="SaveToDisk"/>.
        /// </summary>
        void SaveToDisk();
    }
}
