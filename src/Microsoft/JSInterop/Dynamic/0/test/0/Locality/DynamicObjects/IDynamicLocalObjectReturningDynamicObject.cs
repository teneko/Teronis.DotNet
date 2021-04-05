// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Teronis.Microsoft.JSInterop.Annotations;

namespace Teronis.Microsoft.JSInterop.Locality.DynamicObjects
{
    public interface IDynamicLocalObjectReturningDynamicObject
    {
        [ReturnDynamicLocalObject]
        public ValueTask<ILocalObjectReturningDynamicObject> GetDynamicLocalObject();
    }
}
