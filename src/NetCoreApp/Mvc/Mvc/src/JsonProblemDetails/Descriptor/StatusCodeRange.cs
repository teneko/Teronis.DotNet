using System.Collections;
using System.Collections.Generic;

namespace Teronis.Mvc.JsonProblemDetails.Descriptor
{
    public sealed class StatusCodeRange : IEnumerable<int>
    {
        public int Begin { get; }
        public int End { get; }

        public StatusCodeRange(int begin, int end)
        {
            Begin = begin;
            End = end;
        }

        public IEnumerator<int> GetEnumerator()
        {
            yield return Begin;
            yield return End;
        }

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}
