

namespace Teronis.Identity.Presenters
{
    public interface IServiceResultFactory
    {
        IServiceResultFactory WithStatusCode(int? statusCode);
    }
}
