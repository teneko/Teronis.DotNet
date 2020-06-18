namespace Teronis.Mvc.ServiceResulting
{
    public interface IMutableServiceResult
    {
        bool Succeeded { get; set; }
        object? Content { get; set; }
        int? StatusCode { get; set; }
        JsonErrors? Errors { get; set; }
    }
}
