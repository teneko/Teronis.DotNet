using System;

namespace Teronis
{
    public interface ICloneable<T> : ICloneable
    {
        new T Clone();
    }
}