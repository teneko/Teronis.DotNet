using Force.Crc32;
using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.NetStandard.Extensions
{
    public static class ArrayByteExtensions
    {
        public static uint GenerateChecksum(this byte[] data) => Crc32CAlgorithm.Compute(data);
    }
}
