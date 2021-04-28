// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;

namespace Teronis.AspNetCore.Mvc
{
    public static class IMvcBuilderExtensions
    {
        public static ControllerAssistance<T> AssistController<T>(this IMvcBuilder mvcBuilder) =>
            ControllerAssistance.AssistController<T>(mvcBuilder);
    }
}
