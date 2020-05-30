using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace Teronis.Identity.Presenters
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
