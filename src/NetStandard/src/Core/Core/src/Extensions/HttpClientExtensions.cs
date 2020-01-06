using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Teronis.Extensions
{
    public static class HttpClientExtensions
    {
        #region PrepareHttpRequest

        public static async Task<T> PrepareHttpRequestAsync<T>(this HttpClient client, HttpClientHandler clientHandler, Func<HttpClient, HttpClientHandler, Task<T>> callback)
        {
            if (callback != null)
                return await callback.Invoke(client, clientHandler);

            return default;
        }

        public static async Task<T> PrepareHttpRequestAsync<T>(this HttpClient pseudoClient, Func<HttpClient, Task<T>> clientHandler)
            => await PrepareHttpRequestAsync(pseudoClient, null, async (client, handler) => await clientHandler?.Invoke(client));

        public static async Task PrepareHttpRequestAsync(this HttpClient pseudoClient, HttpClientHandler clientHandler, Func<HttpClient, HttpClientHandler, Task> callback)
        {
            await PrepareHttpRequestAsync(pseudoClient, clientHandler, async (_client, _handler) => {
                if (callback != null)
                    await callback.Invoke(_client, clientHandler);

                return default(object);
            });
        }

        public static async Task PrepareHttpRequestAsync(this HttpClient pseudoClient, Func<HttpClient, Task> clientHandler)
            => await PrepareHttpRequestAsync(pseudoClient, null, async (client, handler) => await clientHandler?.Invoke(client));

        #endregion
    }
}
