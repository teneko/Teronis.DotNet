

namespace Teronis.Identity.Presenters
{
    public interface IMutableServiceResult
    {
        bool Succeeded { get; set; }
        object? Content { get; set; }
        JsonErrors? Errors { get; set; }
    }
}
