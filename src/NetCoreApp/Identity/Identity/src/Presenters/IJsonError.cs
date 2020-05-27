using System;

namespace Teronis.Identity.Presenters
{
    public interface IJsonError
    {
        public string ErrorCode { get; }
        public Exception? Error { get; }
    }
}
