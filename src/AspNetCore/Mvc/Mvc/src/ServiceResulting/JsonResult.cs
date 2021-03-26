// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace Teronis.Mvc.ServiceResulting
{
    public class JsonResult : ObjectResult
    {
        /// <summary>
        /// If not specified, the formatters from startup are used by default.
        /// </summary>
        public new FormatterCollection<IOutputFormatter>? Formatters {
            get => base.Formatters;
            set => base.Formatters = value;
        }

        public JsonResult() : base(null) =>
            ContentTypes.Add(new MediaTypeHeaderValue("application/json")); // json result

        public override string? ToString() =>
            Value?.ToString() ?? null;
    }
}
