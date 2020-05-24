

namespace Teronis.Data
{
    public interface IWorking
    {
        bool IsWorking { get; }
        
        void BeginWork();
        void EndWork();
    }
}
