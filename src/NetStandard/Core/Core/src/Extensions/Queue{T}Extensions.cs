using System.Collections.Generic;

namespace Teronis.Extensions
{
    public static class QueueExtensions
    {
        public static IEnumerable<T> DequeueChunk<T>(this Queue<T> queue, int chunkSize)
        {
            for (int i = 0; i < chunkSize && queue.Count > 0; i++)
                yield return queue.Dequeue();
        }

        public static T DequeueAt<T>(this Queue<T> queue, int index)
        {
            for (int i = 0; i < index && queue.Count > 0; i++)
                queue.Dequeue();

            if (queue.Count != 0)
                return queue.Dequeue();
            else
                return default;
        }
    }
}
