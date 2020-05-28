using Microsoft.AspNetCore.Mvc.Formatters;
using Teronis.Extensions;

namespace Teronis.Identity.Presenters
{
    public class ServiceResult : JsonResult, IServiceResult, IMutableServiceResult
    {
        public JsonErrors? Errors {
            get => Value is JsonErrors value ? value : default;
            private set => Value = value;
        }

        public new object? Value {
            get => base.Value is JsonErrors ? default : base.Value;
            private set => base.Value = value;
        }

        public bool Succeeded { get; private set; }

        bool IMutableServiceResult.Succeeded {
            get => Succeeded;
            set => Succeeded = value;
        }

        object? IMutableServiceResult.Value {
            get => Value;
            set => base.Value = value;
        }

        /// <summary>
        /// Creates a new service result from <paramref name="datransject"/>.
        /// </summary>
        /// <param name="datransject">The data with what this instance get filled. Noting will be deep copied.</param>
        internal ServiceResult(in ServiceResultDatransject datransject)
        {
            Succeeded = datransject.Succeeded;
            Value = datransject.Value;
            Errors = datransject.Errors;
            DeclaredType = datransject.DeclaredType;
            Formatters = datransject.Formatters;
            ContentTypes = datransject.ContentTypes;
            StatusCode = datransject.StatusCode;
        }

        public ServiceResult(bool succeeded, object? content = null, int? statusCode = null)
        {
            Succeeded = succeeded;
            Value = content;
            StatusCode = statusCode;
        }

        public ServiceResultDatransject DeepCopy()
        {
            return new ServiceResultDatransject(Succeeded, Value, Errors is null ? null : new JsonErrors().Include(Errors),
                DeclaredType, Formatters is null ? null : new FormatterCollection<IOutputFormatter>().Include(Formatters),
                ContentTypes is null ? null : new MediaTypeCollection().Include(ContentTypes), StatusCode);
        }
    }
}
