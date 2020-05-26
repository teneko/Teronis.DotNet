

namespace Teronis.Identity.Presenters
{
    public abstract class ServiceResultBase : JsonResult, IServiceResult
    {
        public JsonErrors? Errors {
            get => Value is JsonErrors value ? value : default;
            set => Value = value;
        }

        public bool Succeeded { get; }

        protected ServiceResultBase(bool succeeded, object? content = null, int? statusCode = null)
        {
            Succeeded = succeeded;
            Value = content;
            StatusCode = statusCode;
        }
    }
}
