using System;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.AspNetCore.Identity.Entities
{
    public interface IBearerTokenEntity
    {
        public Guid BearerTokenId { get; }
        [NotNull]
        public string UserId { get; }
        public DateTime CreatedAtUtc { get; }
        public DateTime ExpiresAtUtc { get; }
    }
}
