// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.ComponentModel.Parenthood
{
    public interface IHaveRegisteredParents : IHaveParents
    {
        void RegisterParent(object caller, ParentsRequestedEventHandler handler);
        void RegisterParent(ParentsRequestedEventHandler handler);
        void UnregisterParent(object caller);
        void UnregisterParent(ParentsRequestedEventHandler handler);
    }
}
