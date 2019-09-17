using System;
using Teronis.Data;

namespace Teronis.Extensions.NetStandard
{
    public static class UpdatingEventArgsExtensions
    {
        /// <summary>
        /// This function invokes <see cref="UpdatingEventHandler{T}"/> and returns <see cref="UpdatingEventArgs{T}.Handled"/>.
        /// </summary>
        public static bool IsUpdateAppliable<T>(this UpdatingEventArgs<T> args, object sender, UpdatingEventHandler<T> handler)
        {
            handler?.Invoke(sender, args);
            return !args.Handled;
        }
    }
}
