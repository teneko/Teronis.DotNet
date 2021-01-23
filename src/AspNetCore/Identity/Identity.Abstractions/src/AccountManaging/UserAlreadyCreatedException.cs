using System;
using System.Runtime.Serialization;

namespace Teronis.AspNetCore.Identity.AccountManaging
{
    public class UserAlreadyCreatedException : ArgumentException
    {
        public string? UserName { get; }

        public UserAlreadyCreatedException(string? userName) =>
            UserName = userName;

        public UserAlreadyCreatedException(string? userName, string? message)
            : base(message) =>
            UserName = userName;

        public UserAlreadyCreatedException(string? userName, string? message, Exception? innerException)
            : base(message, innerException) =>
            UserName = userName;

        protected UserAlreadyCreatedException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
