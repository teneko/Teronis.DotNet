// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Teronis.ObjectModel
{
    public abstract class DataErrorInfosBase : INotifyDataErrorInfo
    {
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public virtual bool HasErrors => ErrorInfos.Count > 0;

        protected virtual Dictionary<string, ICollection<string>> ErrorInfos { get; }

        public DataErrorInfosBase() =>
            ErrorInfos = new Dictionary<string, ICollection<string>>();

        public virtual IEnumerable GetErrors(string? propertyName)
        {
            if (propertyName == null || propertyName == string.Empty || !ErrorInfos.ContainsKey(propertyName)) {
                return Enumerable.Empty<string>();
            }

            return ErrorInfos[propertyName];
        }

        protected virtual void OnErrorsChanged(string propertyName) =>
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

        public virtual void SetErrors(string propertyName, ICollection<string> errors)
        {
            ErrorInfos[propertyName] = errors;
            OnErrorsChanged(propertyName);
        }

        public virtual void RemoveErrors(string propertyName)
        {
            if (ErrorInfos.ContainsKey(propertyName)) {
                ErrorInfos.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }
    }
}
