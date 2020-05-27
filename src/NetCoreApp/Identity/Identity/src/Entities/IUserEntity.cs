using System;
using System.Collections.Generic;

namespace Teronis.Identity.Entities
{
    public interface IUserEntity
    {
        string Id { get; }
        string UserName { get; }
        string SecurityStamp { get; }
    }
}
