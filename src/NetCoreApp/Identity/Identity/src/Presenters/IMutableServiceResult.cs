

namespace Teronis.Identity.Presenters
{
    public interface IMutableServiceResult
    {
        bool Succeeded { get; set; }
        object? Value { get; set; }
    }
}
