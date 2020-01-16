

namespace Teronis
{
    public interface IChild<T> where T : IChild<T>
    {
        T TryGetSubLayer();
    }
}
