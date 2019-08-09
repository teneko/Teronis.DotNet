using System;
using System.Collections.Generic;
using System.Text;
using Teronis.Data;

namespace Teronis.Extensions.NetStandard
{
    public static class UpdateExtensions
    {
        public static IUpdate<object> GetObjectifiedUpdate<ContentType>(this IUpdate<ContentType> update)
        {
            if (update != null)
                return new Update<object>(update.Content, update.UpdateCreationSource);
            else
                return null;
        }
    }
}
