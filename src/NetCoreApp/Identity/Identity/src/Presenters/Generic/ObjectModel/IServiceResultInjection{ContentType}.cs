

using Teronis.Identity.Presenters.Generic;

namespace Teronis.Identity.Presenters.Generic.ObjectModel
{
    public interface IServiceResultInjection<in ContentType>
    {
        void SetResult(IServiceResult<ContentType> value);
    }
}
