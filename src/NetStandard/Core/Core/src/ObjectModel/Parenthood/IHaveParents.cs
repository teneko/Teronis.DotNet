// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.ObjectModel.Parenthood
{
    public interface IHaveParents
    {
        event ParentsRequestedEventHandler ParentsRequested;

        ParentsCollector CreateParentsCollector();
    }
}
