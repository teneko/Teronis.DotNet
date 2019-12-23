using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis
{
    public interface IRepositoryResolver
    {
        RepositoryType GetRepository<EntityType, RepositoryType>() where RepositoryType : IRepository<EntityType>;
    }
}
