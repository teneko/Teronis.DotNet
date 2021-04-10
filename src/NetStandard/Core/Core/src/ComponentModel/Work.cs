// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Teronis.ComponentModel
{
    public class Work : INotifyPropertyChanged, IWorking
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public bool IsWorking =>
            workLevel > 0;

        private int workLevel;

        public Work() { }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public IDisposable BeginWork()
        {
            if (workLevel++ == 0) {
                OnPropertyChanged(nameof(IsWorking));
            }

            return new DisposableWork(this);
        }

        private void endWork()
        {
            if (workLevel == 0) {
                throw new Exception("You cannot end the work before it has not begun.");
            }

            if (--workLevel == 0) {
                OnPropertyChanged(nameof(IsWorking));
            }
        }

        private class DisposableWork : IDisposable
        {
            private readonly Work workStatus;

            public DisposableWork(Work workStatus)
            {
                this.workStatus = workStatus;
            }

            public void Dispose() =>
                workStatus.endWork();
        }
    }
}
