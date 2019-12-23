using MorseCode.ITask;
using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Data
{
    public interface IContentUpdate<out ContentType> : IContentUpdate
    {
        new ContentType Content { get; }
        new ITask<ContentType> ContentTask { get; }
    }
}
