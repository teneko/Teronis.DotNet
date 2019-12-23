using Force.Crc32;

namespace Teronis.Extensions.NetStandard
{
    public static class ArrayByteExtensions
    {
        public static uint GenerateChecksum(this byte[] data) => Crc32CAlgorithm.Compute(data);
    }
}
