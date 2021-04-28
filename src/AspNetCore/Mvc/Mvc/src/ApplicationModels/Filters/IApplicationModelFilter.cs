// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Teronis.AspNetCore.Mvc.ApplicationModels.Filters
{
    public interface IApplicationModelFilter
    {
        bool IsAllowed(ApplicationModel appliaction);
    }
}
