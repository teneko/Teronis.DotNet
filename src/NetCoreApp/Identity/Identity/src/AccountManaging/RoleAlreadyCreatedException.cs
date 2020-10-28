using System;
using System.Runtime.Serialization;

namespace Teronis.Identity.AccountManaging
{
    public class RoleAlreadyCreatedException : ArgumentException
    {
        public string? RoleName { get; }

        public RoleAlreadyCreatedException(string? roleName) =>
            RoleName = roleName;

        public RoleAlreadyCreatedException(string? roleName, string? message)
            : base(message) =>
            RoleName = roleName;

        public RoleAlreadyCreatedException(string? roleName, string? message, Exception? innerException)
            : base(message, innerException) =>
            RoleName = roleName;

        protected RoleAlreadyCreatedException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
