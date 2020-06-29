namespace Teronis.Mvc.ServiceResulting
{
    public interface IServiceResultPostConfiguration
    {
        IServiceResultPostConfiguration WithStatusCode(int? statusCode);
    }
}
