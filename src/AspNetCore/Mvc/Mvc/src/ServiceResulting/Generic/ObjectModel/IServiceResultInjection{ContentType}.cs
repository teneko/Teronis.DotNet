// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Teronis.Mvc.ServiceResulting.Generic.ObjectModel
{
    public interface IServiceResultInjection<in ContentType>
    {
        void SetResult(IServiceResult<ContentType> value);
    }
}
