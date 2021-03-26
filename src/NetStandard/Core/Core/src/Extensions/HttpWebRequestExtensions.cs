// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Teronis.Extensions
{
    public static class HttpWebRequestExtensions
    {
        public static Task<WebResponse> GetResponseAsync(this HttpWebRequest request, CancellationToken? nullableCancellationToken)
        {
            CancellationTokenRegistration? nullableCancellationTokenRegistration;

            if (nullableCancellationToken is CancellationToken cancellationToken) {
                nullableCancellationTokenRegistration = cancellationToken.Register(() => request.Abort(), useSynchronizationContext: false);
            } else {
                nullableCancellationTokenRegistration = null;
                cancellationToken = default;
            }

            try {
                return request.GetResponseAsync();
            } catch (WebException error) {
                if (nullableCancellationToken != null && cancellationToken.IsCancellationRequested) {
                    throw new OperationCanceledException(error.Message, error, cancellationToken);
                } else {
                    throw;
                }
            } finally {
                if (nullableCancellationTokenRegistration is CancellationTokenRegistration cancellationTokenRegistration) {
                    cancellationTokenRegistration.Dispose();
                }
            }
        }
    }
}
