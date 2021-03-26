// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

namespace Teronis.IO.FileLocking
{
    [Serializable]
    internal class FileLockException : Exception
    {
        public FileLockException() { }

        public FileLockException(string message) : base(message)
        {
        }

        public FileLockException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FileLockException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
