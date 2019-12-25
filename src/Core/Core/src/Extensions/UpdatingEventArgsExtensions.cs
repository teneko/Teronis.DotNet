using System;
using Teronis.Data;

namespace Teronis.Extensions
{
    public static class UpdatingEventArgsExtensions
    {
        /// <summary>
        /// This function invokes <see cref="ContentUpdatingEventHandler{T}"/> and returns <see cref="ContentUpdatingEventArgs{T}.Handled"/>.
        /// </summary>
        public static bool IsUpdateAppliable<T>(this ContentUpdatingEventArgs<T> args, object sender, ContentUpdatingEventHandler<T> handler)
        {
            handler?.Invoke(sender, args);
            return !args.Handled;
        }
    }
}
