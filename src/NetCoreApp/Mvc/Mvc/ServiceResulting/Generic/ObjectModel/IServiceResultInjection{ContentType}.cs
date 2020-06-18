namespace Teronis.Mvc.ServiceResulting.Generic.ObjectModel
{
    public interface IServiceResultInjection<in ContentType>
    {
        void SetResult(IServiceResult<ContentType> value);
    }
}
