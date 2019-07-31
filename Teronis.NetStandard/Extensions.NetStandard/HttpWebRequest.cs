using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Teronis.Extensions.NetStandard
{
    public class HttpWebRequestExtensions
    {
        public static async Task<HttpWebResponse> GetResponseAsync(this HttpWebRequest request, CancellationToken? nullableCancellationToken) {
            CancellationTokenRegistration? nullableCancellationTokenRegistration;

            if (nullableCancellationToken != null && nullableCancellationToken is CancellationToken cancellationToken)
                nullableCancellationTokenRegistration = cancellationToken.Register(() => request.Abort(), useSynchronizationContext: false);
            else
                nullableCancellationTokenRegistration = null;

            try {
                var response = (HttpWebResponse)await request.GetResponseAsync().ConfigureAwait(false);
                return response;
            } catch (WebException error) {
                if (nullableCancellationToken != null && cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException(error.Message, error, cancellationToken);
                else
                    throw error;
            } finally {
                if (nullableCancellationTokenRegistration != null)
                    cancellationTokenRegistration.dispose
            }
        }
    }
}
