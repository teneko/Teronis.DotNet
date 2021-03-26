// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

namespace Teronis.Diagnostics
{
    [Serializable]
    public class ProcessNotSpawnedException : Exception
    {
        public ProcessNotSpawnedException()
        { }

        public ProcessNotSpawnedException(string message)
            : base(message) { }

        public ProcessNotSpawnedException(string message, Exception innerException)
            : base(message, innerException) { }

        protected ProcessNotSpawnedException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
