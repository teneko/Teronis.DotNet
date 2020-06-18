using System;

namespace Teronis.Mvc.ServiceResulting
{
    public interface IJsonError
    {
        public string ErrorCode { get; }
        public Exception? Error { get; }
    }
}
