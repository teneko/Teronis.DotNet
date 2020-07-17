using System;

namespace Teronis.NetCoreApp.AssemblyLoadInjection
{
    public class InjectionException : Exception
    {
        public InjectionException()
        { }

        public InjectionException(string message)
            : base(message)
        { }

        public InjectionException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
