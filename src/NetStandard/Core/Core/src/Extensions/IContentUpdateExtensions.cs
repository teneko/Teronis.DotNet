using System;
using System.Threading.Tasks;
using MorseCode.ITask;
using Teronis.ObjectModel.Updates;

namespace Teronis.Extensions
{
    public static class IContentUpdateExtensions
    {
        public static ContentUpdate<DeepContentType> CreateUpdateFromContent<ContentType, DeepContentType>(this IContentUpdate<ContentType> update, Func<ITask<ContentType>, DeepContentType> getDeepContent, object updateCreationSource)
            => new ContentUpdate<DeepContentType>(getDeepContent(update.ContentTask), update.OriginalUpdateCreationSource, updateCreationSource);

        public static ContentUpdate<DeepContentType> CreateUpdateFromContent<ContentType, DeepContentType>(this IContentUpdate<ContentType> update, Func<ITask<ContentType>, Task<DeepContentType>> getDeepContent, object updateCreationSource)
            => new ContentUpdate<DeepContentType>(getDeepContent(update.ContentTask), update.OriginalUpdateCreationSource, updateCreationSource);
    }
}
