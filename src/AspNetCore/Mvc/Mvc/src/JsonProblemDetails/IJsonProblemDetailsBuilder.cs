// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;

namespace Teronis.AspNetCore.Mvc.JsonProblemDetails
{
    public interface IJsonProblemDetailsBuilder
    {
        IServiceCollection Services { get; }
    }
}
