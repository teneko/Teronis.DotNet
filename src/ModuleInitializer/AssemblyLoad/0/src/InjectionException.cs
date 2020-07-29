using System;
using System.Runtime.Serialization;

namespace Teronis.ModuleInitializer.AssemblyLoad
{
    [Serializable]
    internal class InjectionException : Exception
    {
        public InjectionException()
        { }

        public InjectionException(string message)
            : base(message)
        { }

        public InjectionException(string message, Exception innerException)
            : base(message, innerException)
        { }

        protected InjectionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
